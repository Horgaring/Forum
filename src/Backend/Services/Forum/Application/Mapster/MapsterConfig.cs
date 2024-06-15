using Application.DTOs.Group;
using Application.DTOs.Post;
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

        TypeAdapterConfig<CreateGroupRequest, Group>.NewConfig()
            .Map(p => p.AvatarPath, p => p.Avatar).TwoWays();

        TypeAdapterConfig<UpdateGroupRequest, Group>.NewConfig()
            .Map(p => p.AvatarPath, p => p.Avatar).TwoWays();

        TypeAdapterConfig<Post, PostResponse>.NewConfig()
            .Map(p => p.GroupId, p => p.Group.Id).TwoWays()
            .Map(p => p.User, p => p.User);
    }
}