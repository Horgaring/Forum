using System.Data;
using Application.DTOs;
using BuildingBlocks;
using Dapper;
using MediatR;


namespace Application.Requests.Post;

public class GetPostRequest : IRequest<List<Domain.Entities.Post>>
{
    
    public int PageSize{ get; set; }
    public int PageNum { get; set; }
}
public class GetPostHandler : IRequestHandler<GetPostRequest,List<Domain.Entities.Post>>
{
    private readonly ISqlconnectionfactory _connectionfactory;

    public GetPostHandler(ISqlconnectionfactory conf)=>
        (_connectionfactory) = (conf);
    
    public async Task<List<Domain.Entities.Post>> Handle(GetPostRequest request, CancellationToken cancellationToken)
    {
        using var con = _connectionfactory.Create();
        
        con.Open();
        var res = await con.QueryAsync<Domain.Entities.Post>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Post"
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                Offset = request.PageNum - 1 * request.PageSize,
                PageSize = request.PageSize
            });
        
        return (res?.ToList() ?? Enumerable.Empty<Domain.Entities.Post>().ToList());

    }
}

