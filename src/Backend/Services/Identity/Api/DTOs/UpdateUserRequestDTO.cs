using Domain;

namespace Api.DTOs;

internal class UpdateUserRequestDTO
{
    public User toUser() => new User()
    {
        UserName = Name,
    };

    public string? Name { get; set; }
}