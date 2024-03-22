using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Group;

public class GetGroupRequestDto
{
    [FromRoute]
    public Guid Id { get; set; }
}