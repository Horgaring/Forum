using Microsoft.AspNetCore.Mvc;

namespace Application;

public class GetPostsByGroupIdRequestDto([FromRoute] Guid groupid,[FromQuery] int pagenum,[FromQuery] int pagesize)
{
    public Guid GroupId {get; set;} = groupid;
    public int PageNum { get;  set; } = pagenum;
    public int PageSize { get;  set; } = pagesize;
}
