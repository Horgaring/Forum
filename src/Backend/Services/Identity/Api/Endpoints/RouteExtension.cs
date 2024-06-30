
using Api.DTOs;
using Domain;
using Infrastructure.Context;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints;

public static class RouteExtension
{
    public static void MapEnpoints(this WebApplication app)
    {
        app.MapPut("api/user", UpdateUser)
            .RequireAuthorization();
        app.MapGet("api/users", GetUser)
            .RequireAuthorization();
        app.MapGet("api/users/{*id:guid}", GetUserById)
            .RequireAuthorization();
        app.MapGet("api/users/{id}/activitys/{days:int}", GetActivity)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetUserById(HttpContext context,
        [FromRoute] Guid id,
        [FromServices] ApplicationDbContext dbContext)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null)
            return Results.BadRequest("User not found");
        return Results.Ok(UserResponseDTO.FromUser(user));
    }
    private static async Task<IResult> GetActivity(HttpContext context,
        [FromRoute] string id,
        [FromRoute] int days,
        [FromServices] ApplicationDbContext dbContext)
    {
        var users = dbContext.DayActivities
            .Where(p => p.UserId == id)
            .OrderBy(p => p.Date)
            .TakeLast(days).ToList();
        if (users is null)
            return Results.BadRequest("User not found");
        return Results.Ok(users);
    }
    
    private static async Task<IResult> UpdateUser(HttpContext context,
        [FromBody] UpdateUserRequestDTO dto,
        [FromServices] ApplicationDbContext db)
    {
        var sub = context.User.Claims.Where(p => p.Type == "sub").First().Value;
        var user =  db.Users.Where(p => p.Id == sub).FirstOrDefault();
        if (user is null)
            return Results.BadRequest("User not found");
        user.UserName = dto.Name;
        db.SaveChanges();
        return Results.Ok();
    }
    private static async Task<IResult> GetUser(HttpContext context,
        [FromServices] ApplicationDbContext db)
    {
        var sub = context.User.Claims.Where(p => p.Type == "sub").First().Value;
        var user =  db.Users.AsNoTracking().Where(p => p.Id == sub).FirstOrDefault();
        if (user is null)
            return Results.BadRequest("User not found");
        return Results.Ok(UserResponseDTO.FromUser(user));
    }
    
}