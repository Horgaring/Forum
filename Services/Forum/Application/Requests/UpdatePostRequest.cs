using Application.Exceptions;
using BuildingBlocks.Core.Repository;
using Grpc.Core;
using Infrastructure.Context;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Requests;

public class UpdatePostRequest : IRequest
{
    public string Userid { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}
public class UpdatePostHandler : IRequestHandler<UpdatePostRequest>
{
    private readonly PostRepository _repository;
    private readonly IUnitOfWork<PostRepository> _uow;

    public UpdatePostHandler(PostRepository repository,IUnitOfWork<PostRepository> uow)=>
        (_repository,_uow) = (repository,uow);


    public async Task Handle(UpdatePostRequest request, CancellationToken cancellationToken)
    {
       var post  = await _repository.SingleOrDefaultAsync(op => op.Userid == request.Userid
                                                 && op.Title == request.Title);
       if (post is null)
       {
           throw new PostNotFountException(StatusCode.NotFound,null);
       }

       post.Update(request.Title,request.Description);
       _repository.Update(post);
       await _uow.CommitAsync();
    }
}
