using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.DTOs.Users
{
    public class ApiUserDto
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ApiUserDtoValidator : AbstractValidator<ApiUserDto>
    {
        public ApiUserDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().NotNull().WithMessage("First Name is required");
            RuleFor(x => x.LastName).NotEmpty().NotNull().WithMessage("LastName is required and cannot be empty");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required and cannot be empty");
            RuleFor(x => x.Password).Length(8, int.MaxValue).WithMessage(
                "Password must be min 8 and max 12 characters").NotEmpty().NotNull().WithMessage("Pasword field is mandatory");
        }
    }
}
