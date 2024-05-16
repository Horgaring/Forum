using Application.Requests;
using Application.Requests.Post;
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
        var command = new GetPostsRequest()
        {
            PageNum = 1,
            PageSize = 1,
        };
        
        var res = await Fixture.SendAsync(command);
        
        Assert.True(res.Count > 0);
    }
}
