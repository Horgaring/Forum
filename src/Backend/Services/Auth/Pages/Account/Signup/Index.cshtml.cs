using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Identityserver.Mediatr.RequestHandler;
using Identityserver.Models;
using Identityserver.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Identityserver.Pages.Account.Signup;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IIdentityProviderStore _identityProviderStore;
    private readonly IMediator _mediator;
    [BindProperty] public InputModel Input { get; set; }

    public Index(
        IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        UserManager<User> userManager,
        IMediator mediator,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _events = events;
        _mediator = mediator;
    }

    public async Task<IActionResult> OnGet(string returnUrl)
    {
        await BuildModelAsync(returnUrl);
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);
        RegisterUserRequest request = new RegisterUserRequest()
        {
            Email = Input.Email,
            Password = Input.Password,
            Username = Input.Username,
            File = Input.File
        };
        await _mediator.Send(request);
        return RedirectToPage("Login", Input.ReturnUrl);
    }

    private async Task BuildModelAsync(string returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;
            Input.Username = context?.LoginHint;
            return;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        
        var allowLocal = true;
        var client = context?.Client;
        if (client != null)
        {
            allowLocal = client.EnableLocalLogin;
        }
    }
}