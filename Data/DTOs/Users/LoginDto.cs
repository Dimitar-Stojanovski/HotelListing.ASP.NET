using FluentValidation;

namespace HotelListing.API.Data.DTOs.Users
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password is required");
        }
    }
}
