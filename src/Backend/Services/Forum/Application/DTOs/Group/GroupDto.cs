using Application.DTOs.Common;
using Domain.Entities;

namespace Application.DTOs.Group;

public class GroupDto
{
    public GroupDto(int followers, List<Domain.Entities.Post> posts, AcountDto owner, string name, Guid id)
    {
        Followers = followers;
        Posts = posts;
        Owner = owner;
        Name = name;
        Id = id;
    }
    
    public GroupDto()
    {
        
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public AcountDto Owner { get; set; }
    
    public List<Domain.Entities.Post> Posts { get; set; }
    
    public int Followers { get; set; }
}