using BuildingBlocks;
using Dapper;
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
        var res = await con.QueryFirstOrDefaultAsync<PostResponse>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Post" AS p
                 Where p.Id = @Id;
                 """,new
            {
                Id = request.Id,
            });
        
        return res;

    }
}
