using System.Net;
using Duende.IdentityServer.Models;
using FluentValidation;
using Identity.Models;
using Identityserver.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Identityserver.Mediatr.RequestHandler;

public class RegisterUserRequest: IRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest>
{
    
    private readonly UserManager<User> _userManager;

    public RegisterUserHandler(UserManager<User> userm)=>
        (_userManager) = (userm);


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
    }
}
public class RegisterNewUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterNewUserValidator()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter the password");
        RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter the username");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter the last email")
            .EmailAddress().WithMessage("A valid email is required");
    }
}
