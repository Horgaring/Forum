using Application.DTOs;
using BuildingBlocks;
using Dapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests;

public class GetCommentRequest : IRequest<List<Comment>>
{
    public Guid ParentId{ get; set; }
    public int ListSize{ get; set; }
    public int ListNum { get; set; }
}

public class GetCommentHsndle : IRequestHandler<GetCommentRequest,List<Comment>>
{
    private readonly ISqlconnectionfactory _Connectionfactory;

    public GetCommentHsndle(ISqlconnectionfactory conf)=>
        (_Connectionfactory) = (conf);
    
    public async Task<List<Comment>> Handle(GetCommentRequest request, CancellationToken cancellationToken)
    {
        using var con = _Connectionfactory.Create();
        
        con.Open();
        var comments = await con.QueryAsync<Comment>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Comment"
                 WHERE "Postid" = @Postid and "Discriminator" = @Descriminator 
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                Offset = (request.ListNum - 1) * request.ListSize,
                PageSize = request.ListSize,
                Postid = request.ParentId,
                Descriminator = nameof(Comment)
            });
        
        return comments.ToList();

    }
}