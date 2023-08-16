using Application.PostRequests;
using FluentValidation;

namespace Application.Validations;

public class CreatePostRequestValidator: AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter the Title");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter the Description");
        
    }
}