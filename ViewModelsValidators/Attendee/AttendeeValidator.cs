using FluentValidation;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee;

namespace WebApplicationForDidacticPurpose.MODELS.ViewModelsValidators.Attendee
{
    public class AttendeeValidator : AbstractValidator<AttendeeViewModel>
    {
        public AttendeeValidator()
        {
            RuleFor(a => a.AttendeeId).NotNull();
            RuleFor(a => a.FirstName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(a => a.LastName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(a => a.Group).NotNull().NotEmpty();
            RuleFor(a => a.Email).NotNull().NotEmpty();
        }
    }
}
