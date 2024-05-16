using BuildingBlocks;
using Dapper;
using MediatR;

namespace Application;

public class GetPostsByGroupIdRequest : IRequest<List<PostResponse>>
{
    public Guid GroupId {get; set;}
    public int PageNum { get; internal set; }
    public int PageSize { get; internal set; }
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
        var res = await con.QueryAsync<PostResponse>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Post" AS p
                 Where p.GroupId = @GroupId
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                GroupId = request.GroupId,
                Offset = request.PageNum - 1 * request.PageSize,
                PageSize = request.PageSize
            });
        
        return (res?.ToList() ?? Enumerable.Empty<PostResponse>().ToList());

    }
}

