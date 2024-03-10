using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs;

public class GetCommentRequestDto
{
    [FromQuery]
    public Guid ParentId{ get; set; }
    [FromQuery]
    public int ListSize{ get; set; }
    [FromQuery]
    public int ListNum { get; set; }
}