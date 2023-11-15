using Application.Exceptions;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Context;
using MediatR;

namespace Application.Requests;

public class CreatePostRequest : IRequest<Post>
{
    
    public string Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
}
public class CreatePostHandler : IRequestHandler<CreatePostRequest,Post>
{
    private readonly PostRepository _repository;
    private readonly IUnitOfWork<PostRepository> _uow;

    public CreatePostHandler(PostRepository repository,IUnitOfWork<PostRepository> uow)=>
        (_repository,_uow) = (repository,uow);


    public async Task<Post> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
        var postentity = new Post()
        {
            Userid = request.Userid,
            Title = request.Title,
            Description = request.Description,
            Date = request.Date
        };
        var post = _repository.SingleOrDefaultAsync(post => post.Userid == postentity.Userid
                                         && post.Title == postentity.Title);
        if (post != null)
        {
            throw new PostAlreadyExistException(StatusCode.AlreadyExists,null);
        }
        await _repository.CreateAsync(postentity);
        await _uow.CommitAsync();
        return postentity;
    }
}