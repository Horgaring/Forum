using Application.Requests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;
using Infrastructure.Seed;

namespace IntegrationTest.Post;

public class UpdateCommentTest:  CommentIntegrationTest
{
    public  UpdateCommentTest(
        TestFixture<Program, CommentDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_update_one_comment_to_db()
    {
        var command = new UpdateCommentRequest(cInitialData.Comment.Content)
        {
            id = cInitialData.Comment.Id,
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}
