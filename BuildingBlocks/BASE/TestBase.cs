using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using ILogger = Serilog.ILogger;
using System.Security.Claims;
using System.Globalization;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace BuildingBlocks.TestBase;



public class TestFixture<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    private readonly WebApplicationFactory<TEntryPoint> _factory;
    private Action<IServiceCollection> TestRegistrationServices { get; set; }
    private PostgreSqlContainer DbTestcontainer;
    private RabbitMqContainer BusTestcontainer;
    public CancellationTokenSource CancellationTokenSource;
    public HttpClient HttpClient
    {
        get
        {
            var httpClient = _factory?.CreateClient();
            return httpClient;
        }
    }

    public GrpcChannel Channel =>
        GrpcChannel.ForAddress(HttpClient.BaseAddress!, new GrpcChannelOptions { HttpClient = HttpClient });

    public IServiceProvider ServiceProvider => _factory?.Services;
    public IConfiguration Configuration => _factory?.Services.GetRequiredService<IConfiguration>();
    public ILogger Logger { get; set; }

    protected TestFixture()
    {
        _factory = new WebApplicationFactory<TEntryPoint>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(AddCustomAppSettings);
                
                builder.ConfigureServices(services =>
                {
                    builder.UseEnvironment("test");
                    TestRegistrationServices?.Invoke(services);
                });
            });
    }

    private void AddCustomAppSettings(WebHostBuilderContext webHostBuilderContext, IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddInMemoryCollection(new KeyValuePair<string, string>[]
        {
            new("MessageBroker:Host", BusTestcontainer.Hostname),
            new("MessageBroker:UserName", "guest"),
            new("MessageBroker:Password", "guest"),
        }!);
    }


    public async Task InitializeAsync()
    {
        CancellationTokenSource = new CancellationTokenSource();
        await StartTestContainerAsync();
    }

    public async Task DisposeAsync()
    {
        await StopTestContainerAsync();
        await _factory.DisposeAsync();
        CancellationTokenSource.Cancel();
    }

    public virtual void RegisterServices(Action<IServiceCollection> services)
    {
        TestRegistrationServices = services;
        
    }
    protected async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = ServiceProvider.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = ServiceProvider.CreateScope();

        var result = await action(scope.ServiceProvider);

        return result;
    }


    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();

            return mediator.Send(request);
        });
    }

    public Task SendAsync(IRequest request)
    {
        return ExecuteScopeAsync(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();
            return mediator.Send(request);
        });
    }
    
    
    private async Task StartTestContainerAsync()
    {
        DbTestcontainer = TestContainers.PostgresTestContainer();
        BusTestcontainer = TestContainers.RabbitMQTestContainer();
        await DbTestcontainer.StartAsync();
        await BusTestcontainer.StartAsync();
    }

    private async Task StopTestContainerAsync()
    {
        await DbTestcontainer.StopAsync();
        await BusTestcontainer.StopAsync();
    }

    public ILogger CreateLogger(ITestOutputHelper outputHelper)
    {
        if (outputHelper != null)
        {
            return new LoggerConfiguration()
                .WriteTo.TestOutput(outputHelper)
                .CreateLogger();
        }
        return null;
    }

    public string? GetDBConnectionString()
    {
        return DbTestcontainer.GetConnectionString();
    }
}

internal class TestContainers
{
    public static PostgreSqlContainer PostgresTestContainer()
    {
        return new PostgreSqlBuilder()
            .WithDockerEndpoint("unix:///var/run/docker.sock")
            .Build();
    }

    public static RabbitMqContainer RabbitMQTestContainer()
    {
        return new RabbitMqBuilder()
            .WithDockerEndpoint("unix:///var/run/docker.sock")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();
    }
}

public class TestWriteFixture<TEntryPoint, TContext> : TestFixture<TEntryPoint>
    where TEntryPoint : class
    where TContext : DbContext
{
    public Task ExecuteDbContextAsync(Func<TContext, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TContext>()));
    }

    public Task ExecuteDbContextAsync(Func<TContext, ValueTask> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TContext>()).AsTask());
    }

    public Task ExecuteDbContextAsync(Func<TContext, IMediator, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TContext>(), sp.GetService<IMediator>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TContext, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TContext>()));
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TContext, ValueTask<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TContext>()).AsTask());
    }

    public Task<T> ExecuteDbContextAsync<T>(Func<TContext, IMediator, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetService<TContext>(), sp.GetService<IMediator>()));
    }

    public Task InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }

            return db.SaveChangesAsync();
        });
    }

    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
        where TEntity : class
        where TEntity2 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);

            return db.SaveChangesAsync();
        });
    }

    public Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3,
        TEntity4 entity4)
        where TEntity : class
        where TEntity2 : class
        where TEntity3 : class
        where TEntity4 : class
    {
        return ExecuteDbContextAsync(db =>
        {
            db.Set<TEntity>().Add(entity);
            db.Set<TEntity2>().Add(entity2);
            db.Set<TEntity3>().Add(entity3);
            db.Set<TEntity4>().Add(entity4);

            return db.SaveChangesAsync();
        });
    }

    public Task<T> FindAsync<T, TKey>(TKey id)
        where T : class
    {
        return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id).AsTask());
    }

    public Task<T> FirstOrDefaultAsync<T>()
        where T : class
    {
        return ExecuteDbContextAsync(db => db.Set<T>().FirstOrDefaultAsync());
    }
}

public class TestReadFixture<TEntryPoint, TRContext> : TestFixture<TEntryPoint>
    where TEntryPoint : class
{
    public Task ExecuteReadContextAsync(Func<TRContext, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TRContext>()));
    }

    public Task<T> ExecuteReadContextAsync<T>(Func<TRContext, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TRContext>()));
    }

    
}

public class TestFixture<TEntryPoint, TContext> : TestWriteFixture<TEntryPoint, TContext>
    where TEntryPoint : class
    where TContext : DbContext
    
{
    public Task ExecuteReadContextAsync(Func<TContext, Task> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TContext>()));
    }

    public Task<T> ExecuteReadContextAsync<T>(Func<TContext, Task<T>> action)
    {
        return ExecuteScopeAsync(sp => action(sp.GetRequiredService<TContext>()));
    }
}

public class TestFixtureCore<TEntryPoint> : IAsyncLifetime
    where TEntryPoint : class
{
    private NpgsqlConnection DefaultDbConnection { get; set; }


    public TestFixtureCore(TestFixture<TEntryPoint> integrationTestFixture, ITestOutputHelper outputHelper)
    {
        Fixture = integrationTestFixture;
        integrationTestFixture.Logger = integrationTestFixture.CreateLogger(outputHelper);
    }

    
    public TestFixture<TEntryPoint> Fixture { get; }


    public async Task InitializeAsync()
    {
        await InitPostgresAsync();
    }

    public async Task DisposeAsync()
    {
        await ResetPostgresAsync();
    }

    private async Task InitPostgresAsync()
    {
        DefaultDbConnection = new NpgsqlConnection(Fixture.GetDBConnectionString());
        await DefaultDbConnection.OpenAsync();
        await SeedDataAsync();
    }

    private async Task ResetPostgresAsync()
    {
        if (DefaultDbConnection is not null)
        {
            await DefaultDbConnection.DisposeAsync();
        }
    }
    

    private async Task SeedDataAsync()
    {
        using var scope = Fixture.ServiceProvider.CreateScope();
        
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAllAsync();
        }
    }
}

public interface IDataSeeder
{
    Task SeedAllAsync();
}

public abstract class TestBase<TEntryPoint, TContext> : TestFixtureCore<TEntryPoint> 
    ,IClassFixture<TestFixture<TEntryPoint, TContext>>
    where TEntryPoint : class
    where TContext : DbContext
{
    protected TestBase(
        TestFixture<TEntryPoint, TContext> integrationTestFixture, ITestOutputHelper outputHelper = null) :
        base(integrationTestFixture, outputHelper)
    {
        Fixture = integrationTestFixture;
        Fixture.RegisterServices((ser) =>
        {
            ser.AddDbContext<TContext>(op => op.UseNpgsql(Fixture.GetDBConnectionString()));
        });
    }

    public TestFixture<TEntryPoint, TContext> Fixture { get; }
}