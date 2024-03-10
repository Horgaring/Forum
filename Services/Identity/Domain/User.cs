

using System.Diagnostics;
using MassTransit.Futures.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser
{
    public DateTime Date { get; set; }
    
    public List<DayActivity> Activitys { get; set; }
}