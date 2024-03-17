using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Group;

public class GetGroupRequestDto()
{
    [FromQuery]
    public string Name { get; set; }
}