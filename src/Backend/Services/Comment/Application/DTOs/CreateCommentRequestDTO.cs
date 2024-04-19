using Domain.ValueObjects;

namespace Application.DTOs;

public class CreateCommentRequestDTO
{
    public Guid Postid { get; set; }
    public string Content { get; init; }
    //public CustomerInfo CustomerInfo { get; set; }
}