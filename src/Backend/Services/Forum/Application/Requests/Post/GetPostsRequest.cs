using System.Data;
using Application.DTOs;
using BuildingBlocks;
using Dapper;
using MediatR;


namespace Application.Requests.Post;

public class GetPostsRequest : IRequest<List<PostResponse>>
{
    
    public int PageSize{ get; set; }
    public int PageNum { get; set; }
}
public class GetPostsHandler : IRequestHandler<GetPostsRequest,List<PostResponse>>
{
    private readonly ISqlconnectionfactory _connectionfactory;

    public GetPostsHandler(ISqlconnectionfactory conf)=>
        (_connectionfactory) = (conf);
    
    public async Task<List<PostResponse>> Handle(GetPostsRequest request, CancellationToken cancellationToken)
    {
        using var con = _connectionfactory.Create();
        
        con.Open();
        var res = await con.QueryAsync<PostResponse>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Post"
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                Offset = (request.PageNum - 1) * request.PageSize,
                PageSize = request.PageSize
            });
        
        return (res?.ToList() ?? Enumerable.Empty<PostResponse>().ToList());

    }
}

