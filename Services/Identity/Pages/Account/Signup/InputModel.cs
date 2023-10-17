// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace Identityserver.Pages.Account.Signup;

public class InputModel
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
    
    [Required] public string Username { get; set; }
    
    public IFormFile File { get; set; }
    
    public string ReturnUrl { get; set; }

    public string Button { get; set; }
    
}