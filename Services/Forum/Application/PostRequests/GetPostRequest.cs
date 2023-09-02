
using BuildingBlocks;
using Dapper;
using GrpcService1;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PostRequests;

public class GetPostRequest : IRequest<List<PostResponseDTO>>
{
    public string Query { get; set; }
    public int PageSize{ get; set; }
    public int PageNum { get; set; }
}
public class GetPostHandler : IRequestHandler<GetPostRequest,List<PostResponseDTO>>
{
    private readonly ISqlconnectionfactory _Connectionfactory;

    public GetPostHandler(ISqlconnectionfactory conf)=>
        (_Connectionfactory) = (conf);
    
    public Task<List<PostResponseDTO>> Handle(GetPostRequest request, CancellationToken cancellationToken)
    {
        using var con = _Connectionfactory.Create();
        
        con.Open();
        var res = con.Query<PostResponseDTO>(
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
        
        return Task.FromResult<List<PostResponseDTO>>(result: res?.ToList() ?? Enumerable.Empty<PostResponseDTO>().ToList());

    }
}

