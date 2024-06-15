using Application.DTOs.Common;
using Application.Exceptions.Post;
using BuildingBlocks;
using Dapper;
using Domain.Entities;
using MediatR;

namespace Application;

public class GetPostRequest : IRequest<PostResponse>
{
    public Guid Id { get; set; }
}
public class GetPostHandler : IRequestHandler<GetPostRequest,PostResponse>
{
    private readonly ISqlconnectionfactory _connectionfactory;

    public GetPostHandler(ISqlconnectionfactory conf)=>
        (_connectionfactory) = (conf);
    
    public async Task<PostResponse> Handle(GetPostRequest request, CancellationToken cancellationToken)
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
                 Where "Post"."Id" = @Id;
                 """,
            (p, c) => 
            {
                p.User = c;
                return p; 
            },
            new
            {
                Id = request.Id,
            });
        if (res is null)
        {
            throw new PostNotFountException(null);
        }
        return res.FirstOrDefault();
    }
}
