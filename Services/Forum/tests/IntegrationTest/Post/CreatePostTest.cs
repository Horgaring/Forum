using Application.PostRequests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Api;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace IntegrationTest.Post;

public class CreatePostTest:  PostIntegrationTest
{
    public  CreatePostTest(
        TestFixture<Program, PostDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_create_new_post_to_db()
    {
        var command = new Application.PostRequests.CreatePostRequest()
        {
            Userid = "Test2",
            Description = "Test2",
            Title = "Test2",
            Date = DateTime.UtcNow
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}

