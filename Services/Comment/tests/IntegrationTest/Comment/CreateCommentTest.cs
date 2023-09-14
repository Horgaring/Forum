
using Application.Requests;
using BuildingBlocks.TestBase;
using Infrastructure.Context;

namespace IntegrationTest.Comment;

public class CreateCommentTest:  CommentIntegrationTest
{
    public  CreateCommentTest(
        TestFixture<Program, CommentDbContext> integrationTestFactory) : base(integrationTestFactory)
    {
    }

    [Fact]
    public async void should_create_new_comment_to_db()
    {
        var command = new CreateCommentRequest("test")
        {
            Postid = Guid.NewGuid()
        };
        
        Exception? exception = await Record.ExceptionAsync(async () => await Fixture.SendAsync(command));
        
        Assert.Null(exception);
    }
}

