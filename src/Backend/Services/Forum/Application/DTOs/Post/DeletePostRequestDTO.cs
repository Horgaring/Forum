using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Post;

public class DeletePostRequestDTO([FromQuery] Guid id)
{
    
    public Guid id { get; set; } = id;
}