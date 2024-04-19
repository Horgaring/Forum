using Application.Requests;
using FluentValidation;

namespace Application.Validations;

public class CreateCommentRequestValidator: AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(x => x.CustomerInfo.Id)
            .NotEqual(Guid.Empty).WithMessage("Invalid Sub");
        
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Please enter the Content");
        
    }
}