using HotelListing.API.Data.DTOs.Users;
using HotelListing.API.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDtp);
        Task<bool> Login(LoginDto loginDto);
    }
}
