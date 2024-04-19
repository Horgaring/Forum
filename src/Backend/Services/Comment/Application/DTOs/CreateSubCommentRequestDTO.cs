using Domain.ValueObjects;

namespace Application.DTOs;

public class CreateSubCommentRequestDTO : CreateCommentRequestDTO
{
    
    public Guid ParentComment { get; set; }
    //public CustomerInfo customerInfo { get; set; }
}