using Application.Requests;
using FluentValidation;

namespace Application.Validations;

public class CreateSubCommentRequestValidator: AbstractValidator<CreateSubCommentRequest>
{
    public CreateSubCommentRequestValidator()
    {
        RuleFor(x => x.CustomerInfo.Id)
            .NotEqual(Guid.Empty).WithMessage("Invalid Sub");
        
        RuleFor(x => x.CustomerInfo.Id)
            .NotEqual(Guid.Empty).WithMessage("Invalid Sub");
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Please enter the Content");
        
    }
}