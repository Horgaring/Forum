using System.Data;
using Application.DTOs;
using Application.DTOs.Common;
using BuildingBlocks;
using Dapper;
using Domain.Entities;
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
        var res = await con.QueryAsync<PostResponse, AccountDto, PostResponse>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Post"
                 INNER JOIN "CustomersId" ON "CustomersId"."Id" = "Post"."UserId"
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,
            (p, c) => 
            {
                p.User = c;
                return p; 
            },
            new
            {
                Offset = (request.PageNum - 1) * request.PageSize,
                PageSize = request.PageSize
            });

        return (res?.ToList() ?? Enumerable.Empty<PostResponse>().ToList());

    }
}

