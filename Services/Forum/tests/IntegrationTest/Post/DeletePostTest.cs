using Application.PostRequests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Infrastructure.Seed;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace IntegrationTest.Post;

public class DeletePostTest:  PostIntegrationTest
{
    public  DeletePostTest(
        TestFixture<Program, PostDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_delete_post_to_db()
    {
        var command = new Application.PostRequests.DeletePostRequest()
        {
            id = pInitialData.Post.Id
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}
