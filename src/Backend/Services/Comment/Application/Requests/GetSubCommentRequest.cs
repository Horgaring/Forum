using Application.DTOs;
using BuildingBlocks;
using Dapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests;

public class GetSubCommentRequest : IRequest<List<SubComment>>
{
    public Guid ParentId{ get; set; }
    public int ListSize{ get; set; }
    public int ListNum { get; set; }
}

public class GetSubCommentHsndle : IRequestHandler<GetSubCommentRequest,List<SubComment>>
{
    private readonly ISqlconnectionfactory _Connectionfactory;

    public GetSubCommentHsndle(ISqlconnectionfactory conf)=>
        (_Connectionfactory) = (conf);
    
    public async Task<List<SubComment>> Handle(GetSubCommentRequest request, CancellationToken cancellationToken)
    {
        using var con = _Connectionfactory.Create();
        
        con.Open();
        var comments = await con.QueryAsync<SubComment>(
            sql: """
                 SELECT
                     *
                 FROM
                    "Comment"
                 WHERE  "ParentComment" = @ParentComment and "Discriminator" = @Descriminator 
                 OFFSET @Offset ROWS
                 FETCH NEXT @PageSize ROWS ONLY;
                 """,new
            {
                Offset = (request.ListNum - 1) * request.ListSize,
                PageSize = request.ListSize,
                ParentComment = request.ParentId,
                Descriminator = nameof(SubComment)
            });
        
        return comments.ToList();

    }
}