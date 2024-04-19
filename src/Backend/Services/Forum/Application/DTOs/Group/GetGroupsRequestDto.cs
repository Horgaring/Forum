using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Group;

public class GetGroupsRequestDto()
{
    [FromQuery]
    public string Name { get; set; }
    [FromQuery]
    public int Page { get; set; }
    [FromQuery]
    public int Size { get; set; }
}