using Application.Requests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;

namespace IntegrationTest.Comment;

public class DeleteCommentTest:  CommentIntegrationTest
{
    public  DeleteCommentTest(
        TestFixture<Program, CommentDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_delete_comment_to_db()
    {
        var command = new DeleteCommentRequest()
        {
            id = Infrastructure.Seed.cInitialData.Comment.Id
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}
