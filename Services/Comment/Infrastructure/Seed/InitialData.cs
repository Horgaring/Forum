using Domain;
using Domain.Entities;

namespace Infrastructure.Seed;

public class cInitialData
{
    private static readonly DateTime _date = DateTime.UtcNow;
    private static readonly Guid _postid = Guid.NewGuid();
    private static readonly Guid _id = Guid.NewGuid();
    public static Comment Comment
    {
        get
        {
            Comment comment = new Comment("Test")
            {
                Id = _id,
                Date = _date,
                Postid = _postid
            };
            return comment;
        }
    }
}