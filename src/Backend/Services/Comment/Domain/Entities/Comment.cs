using BuildingBlocks;
using BuildingBlocks.Core.Model;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Comment : AggregateRoot<Guid>
{
    public Comment() { }
    public Comment(string content)
    {
        Content = content;
    }
    public Guid Postid { get; set; }
    
    public CustomerInfo CustomerInfo { get; set; }
    
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdate { get; set; }
    public void Update(string content)
    {
        LastUpdate = DateTime.UtcNow;
        Content = content;
    }

}