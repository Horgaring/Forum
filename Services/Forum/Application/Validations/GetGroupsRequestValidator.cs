using Application.Requests.Group;
using FluentValidation;

namespace Application.Validations;

public class GetGroupsRequestValidator : AbstractValidator<GetGroupsRequest>
{
    public  GetGroupsRequestValidator()
    {
        RuleFor(p => p.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");
        
        RuleFor(p => p.Size)
            .GreaterThan(0).WithMessage("Size must be greater than 0");
    }
}