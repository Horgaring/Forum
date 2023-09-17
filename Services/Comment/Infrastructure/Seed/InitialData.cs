using Domain;

namespace Infrastructure.Seed;

public class cInitialData
{
    private static readonly DateTime _date = DateTime.UtcNow;
    private static readonly Guid _postid = Guid.NewGuid();
    public static Comment Comment
    {
        get
        {
            Comment comment = new Comment("Test")
            {
                Id = 1,
                Date = _date,
                Postid = _postid
            };
            return comment;
        }
    }
}