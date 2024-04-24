using Application.Requests;
using Application.Requests.Post;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Infrastructure.Seed;

namespace IntegrationTest.Post;

public class UpdatePostTest:  PostIntegrationTest
{
    public  UpdatePostTest(
        TestFixture<Program, PostDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_update_one_post_to_db()
    {
        var command = new UpdatePostRequest()
        {
            Description = InitialData.Post.Description,
            Title = InitialData.Post.Title,
            User = InitialData.Post.User
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}
