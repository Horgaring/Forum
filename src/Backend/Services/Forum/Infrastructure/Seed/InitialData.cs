using Domain;
using Domain.Entities;
using Npgsql.Replication;

namespace Infrastructure.Seed;

/// <summary>
/// Initial Data Seeder
/// </summary>
public static class InitialData
{
    private readonly static Guid _comonid = Guid.NewGuid();
    private readonly static Guid _groupid = Guid.NewGuid();
    private readonly static Guid _custid = Guid.NewGuid();
    public static Post? Post
    {
        get
        {
        
            var post = new Post();
            post.Id = _comonid;
            post.Description = "Test";
            post.CreatedAt = DateTime.UtcNow;
            post.Title = "Test";
            return post;
        }
    }

    public static Group Group
    {
        get
        {
            var Customer = CustomerId;
            var group = new Group();
            group.AvatarPath = new byte[0];
            group.Id = _groupid;
            group.Name = "Test";
            
            return group;
        }
    }
    public static CustomerId CustomerId
    {
        get{
            return new CustomerId(){
                Name = "Test",
                Id = _custid
            }; 
        }
    }
}