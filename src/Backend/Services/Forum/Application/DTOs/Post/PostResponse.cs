﻿using Domain.Entities;

namespace Application;

public class PostResponse
{
    public CustomerId User { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public Guid GroupId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdate { get; set; }
}
