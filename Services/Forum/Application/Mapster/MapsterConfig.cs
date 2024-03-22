using Application.DTOs.Group;
using Application.Requests;
using Application.Requests.Post;
using BuildingBlocks.Core.Events.Post;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Mapster;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<Post, CreatedPostEvent>.NewConfig()
            .Map(p => p.Userid, p => p.User.Id).TwoWays();


        TypeAdapterConfig<CreatePostRequest, Post>.NewConfig()
            .Map(p => p.CreatedAt, _ => DateTime.UtcNow)
            .Map(p => p.LastUpdate, _ => DateTime.UtcNow);

        TypeAdapterConfig<CreateGroupRequestDto, CreateGroupRequest>.NewConfig()
            .Map(p => p.Avatar, p => p.Avatar).Ignore();
    }
}