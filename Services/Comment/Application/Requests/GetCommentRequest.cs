using GrpcService1;
using BuildingBlocks;
using Dapper;
using MediatR;
using GrpcService1;
namespace Application.Requests;

public class GetCommentRequest : IRequest<List<GrpcService1.CommentResponseGrpc>>
{
    public string PostId { get; set; }
    public int ListSize{ get; set; }
    public int ListNum { get; set; }
}

public class GetCommentHandler : IRequestHandler<GetCommentRequest,List<GrpcService1.CommentResponseGrpc>>
{
    private readonly ISqlconnectionfactory _Connectionfactory;

    public GetCommentHandler(ISqlconnectionfactory conf)=>
        (_Connectionfactory) = (conf);
    
    public Task<List<GrpcService1.CommentResponseGrpc>> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        using var con = _Connectionfactory.Create();
        
        con.Open();
        var comments = con.Query<GrpcService1.CommentResponseGrpc>(
            sql: """
                 SELECT
                     Postid,
                     Content,
                     Date
                 FROM
                    "Comment"
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                Offset = request.ListNum * request.ListSize,
                PageSize = request.ListSize
            });
        
        return Task.FromResult<List<GrpcService1.CommentResponseGrpc>>(result: comments.ToList());

    }
}

