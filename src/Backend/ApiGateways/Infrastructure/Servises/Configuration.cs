using BuildingBlocks;
using BuildingBlocks.Extension;
using Infrastructure.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Clients;

public static class Configuration
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection service)
    {
        
        var commentoption = service.GetOptions<CommentGrpcOption>();
        var postoption = service.GetOptions<PostGrpcOption>();
        service.AddGrpcClient<GrpcClientcomment.Comment.CommentClient>(op =>
            op.Address = new Uri(commentoption.Сonnection));
        service.AddGrpcClient<GrpcClientpost.Post.PostClient>(op =>
            op.Address = new Uri(postoption.Сonnection));
        service.AddSingleton<CommentGrpcService>();
        service.AddSingleton<PostGrpcService>();
        return service;
    }
}