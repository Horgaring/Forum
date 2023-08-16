using Domain;

using MediatR;
using Infrastructure.Context;

namespace Application.PostRequests;

public class CreatePostRequest : IRequest
{
    
    public string Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}
public class CreatePostHandler : IRequestHandler<CreatePostRequest>
{
    private readonly PostDbContext _db;

    public CreatePostHandler(PostDbContext db)=>
        (_db) = (db);


    public async Task Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        
        var postentity = new Post()
        {
            Userid = request.Userid,
            Title = request.Title,
            Description = request.Description,
            Date = request.Date
        };
        _db.Post.Add(postentity);
        await _db.SaveChangesAsync();
        

    }
}