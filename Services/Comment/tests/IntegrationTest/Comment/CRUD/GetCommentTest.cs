using Application.Requests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;

namespace IntegrationTest.Comment;

public class GetCommentTest:  CommentIntegrationTest
{
    public  GetCommentTest(
        TestFixture<Program, CommentDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_get_one_comment_to_db()
    {
        var command = new GetCommentRequest()
        {
            ListNum = 1,
            ListSize = 1,
            PostId = "Test"
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}
