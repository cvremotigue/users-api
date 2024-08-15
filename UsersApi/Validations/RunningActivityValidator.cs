using FluentValidation;
using UsersApi.Features.RunningActivity;

namespace UsersApi.Validations
{
    public class CreateRunningActivityValidator: AbstractValidator<CreateRunningActivityRequest>
    {
        public CreateRunningActivityValidator() 
        {
            RuleFor(x => x.Distance)
                .NotEqual(0)
                .WithMessage("Invalid Distance.");

            RuleFor(x => x.StartDate)
                .LessThan(y => y.EndDate)
                .LessThan(DateTimeOffset.UtcNow)
                .WithMessage("Invalid StartDate.");

            RuleFor(x => x.EndDate)
                .GreaterThan(y => y.StartDate)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage("Invalid EndDate.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .NotNull()
                .WithMessage("Location required");

            RuleFor(x => x.UserId)
                .NotEqual(default(Guid))
                .WithMessage("Invalid UserId.");
        }
    }

    public class EditRunningActivityValidator : AbstractValidator<EditRunningActivityRequest>
    {
        public EditRunningActivityValidator()
        {
            RuleFor(x => x.Distance)
                .NotEqual(0)
                .WithMessage("Invalid Distance.");

            RuleFor(x => x.StartDate)
                .LessThan(y => y.EndDate)
                .LessThan(DateTimeOffset.UtcNow)
                .WithMessage("Invalid StartDate.");

            RuleFor(x => x.EndDate)
                .GreaterThan(y => y.StartDate)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage("Invalid EndDate.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .NotNull()
                .WithMessage("Location required");
        }
    }
}
