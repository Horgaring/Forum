using Application.DTOs.Common;
using Domain.Entities;

namespace Application.DTOs.Group;

public class GroupDto
{
    public GroupDto(int followers, AcountDto owner, string name, Guid id,byte[] avatar)
    {
        Followers = followers;
        Owner = owner;
        Name = name;
        Id = id;
        Avatar = avatar;
    }
    
    public GroupDto()
    {
        
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public byte[] Avatar { get; set; }
    public AcountDto Owner { get; set; }
    public int Followers { get; set; }
}