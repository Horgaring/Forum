// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Identityserver.Pages.Account.Signup;

public class InputModel
{
    [Required]
    [EmailAddress] 
    public string Email { get; set; }

    [Required] 
    [MinLength(6)]
    [MaxLength(15)]
    public string Password { get; set; }
    
    [Required] 
    [MaxLength(10)]
    [MinLength(3)]
    public string Username { get; set; }
    
    [Required] 
    public IFormFile File { get; set; }
    public string ReturnUrl { get; set; }

    public string Button { get; set; }
    
}