using Application.Exceptions;
using Application.Exceptions.Common;
using Application.Exceptions.Post;
using BuildingBlocks.Core.Repository;
using Domain.Entities;
using Grpc.Core;
using Infrastructure.Context;
using MediatR;

namespace Application.Requests.Post;

public class UpdatePostRequest : IRequest
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public CustomerId User { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }

    public byte[]? Content { get; set; }
}
public class UpdatePostHandler : IRequestHandler<UpdatePostRequest>
{
    private readonly PostRepository _repository;
    private readonly IUnitOfWork<PostDbContext> _uow;

    public UpdatePostHandler(PostRepository repository,IUnitOfWork<PostDbContext> uow)=>
        (_repository,_uow) = (repository,uow);


    public async Task Handle(UpdatePostRequest request, CancellationToken cancellationToken)
    {
       var post  = await _repository.SingleOrDefaultAsync(op => op.Id == request.Id);
       
       if (post is null)
       {
           throw new PostNotFountException(null);
       }
       post.Update(request.Title,request.Description,request.Content);
       _repository.Update(post);
       await _uow.CommitAsync();
    }
}
