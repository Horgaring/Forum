using Application.Requests;
using FluentValidation;

namespace Application.Validations;

public class GetPostRequestValidator: AbstractValidator<GetPostRequest>
{
    public GetPostRequestValidator()
    {
        RuleFor(x => x.Query).NotEmpty().WithMessage("Please enter the Query");
        RuleFor(x => x.PageSize).NotEmpty().WithMessage("Please enter the pagesize")
            .GreaterThan(0).WithMessage("Value pagesize must be greater than 0");;
        RuleFor(x => x.PageNum).NotEmpty().WithMessage("Please enter the pagenum")
            .GreaterThan(0).WithMessage("Value pagenum must be greater than 0");;
        
    }
}
