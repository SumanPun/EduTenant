using Domain.Entities;
using FluentValidation;

namespace Application.Features.Schools.Validators
{
    internal class UpdateSchoolRequestValidator : AbstractValidator<UpdateSchoolRequest>
    {
        public UpdateSchoolRequestValidator(ISchoolService schoolService)
        {
            RuleFor(request => request.Id)
                .NotEmpty()
                .MustAsync(async (id, ct) => await schoolService.GetSchoolByIdAsync(id) is School schoolInDb && schoolInDb.Id == id)
                    .WithMessage("School does not exist.");

            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("School name is required.")
                .MaximumLength(60)
                    .WithMessage("School name should not exceed 60 characters length.");
            RuleFor(request => request.EstablishedOn)
                .LessThanOrEqualTo(DateTime.Now)
                    .WithMessage("Date established cannot be in future date.");
        }
    }
}
