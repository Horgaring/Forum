using Microsoft.AspNetCore.Mvc;

namespace Application;

public class GetPostsByGroupIdRequestDto([FromRoute] Guid groupid,[FromQuery] int pagenum,[FromQuery] int pagesize)
{
    public Guid GroupId {get; set;}
    public int PageNum { get; internal set; }
    public int PageSize { get; internal set; }
}
