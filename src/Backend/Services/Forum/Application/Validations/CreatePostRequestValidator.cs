using Application.Requests;
using Application.Requests.Post;
using FluentValidation;

namespace Application.Validations;

public class CreatePostRequestValidator: AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Please enter the Title");
        
    }
}