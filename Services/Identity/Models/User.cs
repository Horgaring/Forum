using Microsoft.AspNetCore.Identity;

namespace Identityserver.Models;

public class User : IdentityUser
{
    public DateTime Date { get; set; }
}