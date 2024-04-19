using Application.Requests;
using Application.Validations;
using BuildingBlocks.Validation;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Configuration
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Configuration).Assembly));
        service.AddScoped<IValidator<CreateCommentRequest>, CreateCommentRequestValidator>();
        service.AddScoped<IValidator<CreateSubCommentRequest>, CreateSubCommentRequestValidator>();
        service.AddScoped<IValidator<GetCommentRequest>, GetCommentRequestValidator>();
        
        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        return service;
    }
}