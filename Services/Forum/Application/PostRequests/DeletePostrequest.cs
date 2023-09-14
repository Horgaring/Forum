using System.Net;
using Application.Exceptions;
using Grpc.Core;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PostRequests;

public class DeletePostRequest : IRequest<bool>
{
    public Guid id { get; set; }
}

public class DeletePostHandler : IRequestHandler<DeletePostRequest, bool>
{
    private readonly PostDbContext _db;

    public DeletePostHandler(PostDbContext db)=>
        (_db) = (db);
    
    public async Task<bool> Handle(DeletePostRequest request, CancellationToken cancellationToken)
    {
        var post = await _db.Post.SingleOrDefaultAsync(op => op.Id == request.id);
        if (post is null)
        {
            throw new PostNotFountException(StatusCode.NotFound,null);
        }
        _db.Post.Remove(post);
        if (await _db.SaveChangesAsync() > 0)
        {
            return true;
        }
        return false;
    }
}