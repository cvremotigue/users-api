using FluentValidation;

namespace UsersApi.Features.RunningActivity
{
    public class RunningActivityValidationService
    {
        private readonly IValidator<CreateRunningActivityRequest> _createActivityValidator;
        private readonly IValidator<EditRunningActivityRequest> _editActivityValidator;

        public RunningActivityValidationService(IValidator<CreateRunningActivityRequest> createValidator, IValidator<EditRunningActivityRequest> editValidator )
        { 
            _createActivityValidator = createValidator;
            _editActivityValidator = editValidator;
        }

        public (bool, string) IsValid(CreateRunningActivityRequest request)
        {
            var result = _createActivityValidator.Validate(request);
            var errorMessage = string.Empty;

            if (result.IsValid is false)
            {
                errorMessage = string.Join("", result.Errors.Select(x => x.ErrorMessage));
            }

            return (result.IsValid, errorMessage);
        }

        public (bool, string) IsValid(EditRunningActivityRequest request)
        {
            var result = _editActivityValidator.Validate(request);
            var errorMessage = string.Empty;

            if (result.IsValid is false)
            {
                errorMessage = string.Join("", result.Errors.Select(x => x.ErrorMessage));
            }

            return (result.IsValid, errorMessage);
        }
    }
}
