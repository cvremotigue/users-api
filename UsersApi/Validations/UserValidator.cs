using FluentValidation;
using UsersApi.Features.User;

namespace UsersApi.Validations
{
    public class CreateUserValidator: AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator() 
        {
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.");

            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.");

            RuleFor(user => user.Birthdate)
                .LessThan(DateTimeOffset.UtcNow)
                .WithMessage("Invalid birth date");

            RuleFor(user => user.Weight)
                .NotEqual(0)
                .WithMessage("Invalid weight.");

            RuleFor(user => user.Height)
                .NotEqual(0)
                .WithMessage("Invalid height.");
        }
    }

    public class EditUserValidator: AbstractValidator<EditUserRequest> 
    {
        public EditUserValidator() 
        {
            RuleFor(user => user.FirstName)
                   .NotEmpty()
                   .WithMessage("First name is required.");

            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.");

            RuleFor(user => user.Birthdate)
                .LessThan(DateTimeOffset.UtcNow)
                .WithMessage("Invalid birth date");

            RuleFor(user => user.Weight)
                .NotEqual(0)
                .WithMessage("Invalid weight.");

            RuleFor(user => user.Height)
                .NotEqual(0)
                .WithMessage("Invalid height.");
        }
    }
}
