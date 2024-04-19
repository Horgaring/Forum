using Domain;

namespace Api.DTOs;

internal class UserResponseDTO
{
    public static UserResponseDTO FromUser(User user) => new UserResponseDTO()
    {
        Name = user.UserName,
        Email = user.Email,
        CreatedAt = user.Date,
        
    };

    public DateTime CreatedAt { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }
}