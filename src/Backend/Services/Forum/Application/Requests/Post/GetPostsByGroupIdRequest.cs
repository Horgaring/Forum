using Application.DTOs.Common;
using BuildingBlocks;
using Dapper;
using Domain.Entities;
using MediatR;
using Serilog;

namespace Application;

public class GetPostsByGroupIdRequest : IRequest<List<PostResponse>>
{
    public Guid GroupId {get; set;}
    public int PageNum { get;  set; }
    public int PageSize { get;  set; }
}

public class GetPostsByGroupIdHandler : IRequestHandler<GetPostsByGroupIdRequest,List<PostResponse>>
{
    private readonly ISqlconnectionfactory _connectionfactory;

    public GetPostsByGroupIdHandler(ISqlconnectionfactory conf)=>
        (_connectionfactory) = (conf);
    
    public async Task<List<PostResponse>> Handle(GetPostsByGroupIdRequest request, CancellationToken cancellationToken)
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
                 Where "GroupId" = @GroupId
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
                GroupId = request.GroupId,
                Offset = (request.PageNum - 1) * request.PageSize,
                PageSize = request.PageSize
            });
        return (res?.ToList() ?? Enumerable.Empty<PostResponse>().ToList());

    }
}

