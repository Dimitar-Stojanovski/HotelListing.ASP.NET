using HotelListing.API.Data;
using HotelListing.API.Data.DTOs.Users;
using HotelListing.API.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDtp);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<string> GenerateToken(ApiUser user);
    }
}
