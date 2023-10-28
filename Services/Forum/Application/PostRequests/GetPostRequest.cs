
using BuildingBlocks;
using Dapper;
using GrpcService1;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PostRequests;

public class GetPostRequest : IRequest<List<PostResponseGrpc>>
{
    public string Query { get; set; }
    public int PageSize{ get; set; }
    public int PageNum { get; set; }
}
public class GetPostHandler : IRequestHandler<GetPostRequest,List<PostResponseGrpc>>
{
    private readonly ISqlconnectionfactory _Connectionfactory;

    public GetPostHandler(ISqlconnectionfactory conf)=>
        (_Connectionfactory) = (conf);
    
    public Task<List<PostResponseGrpc>> Handle(GetPostRequest request, CancellationToken cancellationToken)
    {
        using var con = _Connectionfactory.Create();
        
        con.Open();
        var res = con.Query<PostResponseGrpc>(
            sql: """
                 SELECT
                     Title,
                     Description,
                     Date,
                     Userid
                 FROM
                    "Post"
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                Offset = request.PageNum * request.PageSize,
                PageSize = request.PageSize
            });
        
        return Task.FromResult<List<PostResponseGrpc>>(result: res?.ToList() ?? Enumerable.Empty<PostResponseGrpc>().ToList());

    }
}

