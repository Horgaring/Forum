using Api;
using BuildingBlocks.Core.Events.Post;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Infrastructure.Seed;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest.Comment;

public class CascadeDeleteTest:  CommentIntegrationTest
{
    public  CascadeDeleteTest(
        TestFixture<Program, CommentDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_Delete_comment_to_db()
    {
        var publisher = Fixture.ServiceProvider.GetService<IPublishEndpoint>();
        
        await publisher?.Publish<DeletedPostEvent>(new DeletedPostEvent()
        {
            PostId = cInitialData.Comment.Postid
        })!;
        var result = await Fixture.ExecuteDbContextAsync(context => 
            Task.FromResult(context.Comment.All(com => 
                com.Postid != cInitialData.Comment.Postid)));
        
        Assert.True(result);
    }
}