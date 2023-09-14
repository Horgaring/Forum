using Application.Exceptions;
using Domain;
using Grpc.Core;
using MediatR;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Application.PostRequests;

public class CreatePostRequest : IRequest<Post>
{
    
    public string Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}
public class CreatePostHandler : IRequestHandler<CreatePostRequest,Post>
{
    private readonly PostDbContext _db;

    public CreatePostHandler(PostDbContext db)=>
        (_db) = (db);


    public async Task<Post> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var postentity = new Post()
        {
            Userid = request.Userid,
            Title = request.Title,
            Description = request.Description,
            Date = request.Date
        };
        var post = _db.Post.SingleOrDefault(post => post.Userid == postentity.Userid
                                         && post.Title == postentity.Title);
        if (post != null)
        {
            throw new PostAlreadyExistException(StatusCode.AlreadyExists,null);
        }
        _db.Post.Add(postentity);
        await _db.SaveChangesAsync();
        return postentity;
    }
}