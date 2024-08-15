using FluentValidation;

namespace UsersApi.Features.User
{
    public class UserValidationService
    {
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly IValidator<EditUserRequest> _editUserValidator;

        public UserValidationService(IValidator<CreateUserRequest> createUserValidator, IValidator<EditUserRequest> editUserValidator)
        {
            _createUserValidator = createUserValidator;
            _editUserValidator = editUserValidator;
        }

        public (bool, string) IsValid(CreateUserRequest request) 
        {
            var result = _createUserValidator.Validate(request);
            var errorMessage = string.Empty;

            if (result.IsValid is false)
            {
                errorMessage = string.Join("", result.Errors.Select(x => x.ErrorMessage));
            }

            return (result.IsValid, errorMessage);
        }

        public (bool, string) IsValid(EditUserRequest request)
        {
            var result = _editUserValidator.Validate(request);
            var errorMessage = string.Empty;

            if (result.IsValid is false)
            {
                errorMessage = string.Join("", result.Errors.Select(x => x.ErrorMessage));
            }

            return (result.IsValid, errorMessage);
        }

    }
}
