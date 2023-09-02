using System.Net;
using Application.Exceptions;
using Grpc.Core;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PostRequests;

public class UpdatePostRequest : IRequest
{
    public string Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}
public class UpdatePostHandler : IRequestHandler<UpdatePostRequest>
{
    private readonly PostDbContext _db;

    public UpdatePostHandler(PostDbContext db)=>
        (_db) = (db);


    public async Task Handle(UpdatePostRequest request, CancellationToken cancellationToken)
    {
       var post  = await _db.Post.SingleOrDefaultAsync(op => op.Userid == request.Userid
                                                 && op.Title == request.Title
                                                 && op.Date == request.Date);
       if (post is null)
       {
           throw new PostNotFountException(StatusCode.NotFound,null);
       }

       post.Update(request.Title,request.Description);
       _db.Post.Update(post);
       await _db.SaveChangesAsync();
    }
}
