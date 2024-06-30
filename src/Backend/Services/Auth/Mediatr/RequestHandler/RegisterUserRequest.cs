using System.Net;
using BuildingBlocks.Core.Events;
using Duende.IdentityServer.Models;
using FluentValidation;
using Identityserver.Exceptions;
using Identityserver.Models;
using Identityserver.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Identityserver.Mediatr.RequestHandler;

public class RegisterUserRequest: IRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile File { get; set; }
}

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest>
{
    
    private readonly UserManager<User> _userManager;
    private readonly IImageService _image;
    
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterUserHandler(UserManager<User> userm,IImageService image, IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
        (_userManager, _image) = (userm, image);
    }


    public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            UserName = request.Username,
            Email = request.Email,
            PasswordHash = request.Password.Sha256(),
            Date = DateTime.UtcNow
        };
        var identityResult = await _userManager.CreateAsync(user, request.Password);
        if (identityResult.Succeeded == false)
        {
            throw new RegisterUserException("failed Register User",
                HttpStatusCode.BadRequest,
                identityResult.Errors.Select(op => op.Description).ToArray());
        }
        if(!_image.TrySaveImage(request.File,user.Id.ToString()))
        {
            await _userManager.DeleteAsync(user);
            throw new RegisterUserException("failed save image",
                HttpStatusCode.BadRequest,
                new []{"failed save image"});
        }
        var userid =  (await _userManager.FindByEmailAsync(user.Email))?.Id;
        if (userid != null)
        {
            Log.Information("Publish UserCreateEvent for {@UserEmail} with id is {@UserID}",
                user.Email,
                user.Id);
            
            await _publishEndpoint.Publish<UserCreatedEvent>(new UserCreatedEvent(user.UserName,Guid.Parse(user.Id)));
            
        }
    }
}