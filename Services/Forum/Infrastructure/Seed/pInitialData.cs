using Domain;
using Domain.Entities;

namespace Infrastructure.Seed;

public static class pInitialData
{
    private readonly static Guid _id = Guid.NewGuid();
    public static Post? Post
    {
        get
        {
            
            var post = new Post();
            post.Id = _id;
            post.Description = "Test";
            post.CreatedAt = DateTime.UtcNow;
            post.Userid.Id = _id;
            post.Title = "Test";
            return post;
        }
    }
}