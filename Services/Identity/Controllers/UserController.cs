using Identityserver.Mediatr.RequestHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers;
[ApiController]
[Route("api/")]
public class UserController : ControllerBase
{
    [HttpPost("users")]
    public async Task<IActionResult> RegisterUser([FromForm]RegisterUserRequest request, IMediator mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return Redirect("Account/Login");
    }
}