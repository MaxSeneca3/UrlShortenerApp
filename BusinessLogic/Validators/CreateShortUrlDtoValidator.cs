using FluentValidation;
using BusinessLogic.Dtos;

namespace BusinessLogic.Validators
{
    public class CreateShortUrlDtoValidator : AbstractValidator<CreateShortUrlDto>
    {
        public CreateShortUrlDtoValidator()
        {
            // Validate OriginalUrl
            RuleFor(x => x.OriginalUrl)
                .NotEmpty().WithMessage("Original URL is required.")
                .Matches(@"^(http|https)://").WithMessage("Original URL must be a valid HTTP or HTTPS URL.");
            
            // Validate UserId
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
            
            // Validate UserName
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name is required.")
                .MaximumLength(50).WithMessage("User Name must not exceed 50 characters.");
            
            // Validate Role
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "Admin" || role == "User").WithMessage("Role must be either 'Admin' or 'User'.");
        }
    }
}