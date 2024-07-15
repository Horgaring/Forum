using BuildingBlocks.Core.Events;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Context.Repository;
using MassTransit;
using Serilog;

namespace Infrastructure.BUS;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly CustomerIdRepository _custrepository;
    private readonly IUnitOfWork _uow;

    public UserCreatedConsumer(CustomerIdRepository custrepository, IUnitOfWork uow)
    {
        _custrepository = custrepository;
        _uow = uow;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        await _custrepository.CreateAsync(new CustomerId(context.Message.Id, context.Message.Name));
        await _uow.CommitAsync();
        Log.Information("Consume UserCreatedEvent for {@USerId}",context.Message.Id);
    }
}