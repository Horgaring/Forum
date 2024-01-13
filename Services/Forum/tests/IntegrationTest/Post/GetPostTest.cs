using Application.Requests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;

namespace IntegrationTest.Post;

public class GetPostTest:  PostIntegrationTest
{
    public  GetPostTest(
        TestFixture<Program, PostDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_get_one_post_to_db()
    {
        var command = new GetPostRequest()
        {
            PageNum = 1,
            PageSize = 1,
            Query = "Test"
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}
