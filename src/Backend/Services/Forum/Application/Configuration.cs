using Application.Mapster;
using Application.Requests;
using Application.Requests.Group;
using Application.Requests.Post;
using Application.Validations;
using BuildingBlocks.Validation;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Configuration
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePostRequest).Assembly));
        service.AddScoped<IValidator<CreatePostRequest>, CreatePostRequestValidator>();
        service.AddScoped<IValidator<GetGroupsRequest>, GetGroupsRequestValidator>();
        service.AddScoped<IValidator<GetPostsRequest>, GetPostRequestValidator>();
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        service.RegisterMapsterConfiguration();
        
        return service;
    }
}