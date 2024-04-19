using Application.Requests;
using FluentValidation;

namespace Application.Validations;

public class GetCommentRequestValidator: AbstractValidator<GetCommentRequest>
{
    public GetCommentRequestValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty().WithMessage("Please enter the Post Id");
        RuleFor(x => x.ListNum)
            .NotEmpty().WithMessage("Please enter the ListNum")
            .GreaterThan(0).WithMessage("Value ListNum must be greater than 0");;
        RuleFor(x => x.ListSize)
            .NotEmpty().WithMessage("Please enter the ListSize")
            .GreaterThan(0).WithMessage("Value ListSize must be greater than 0");;
        
    }
}
