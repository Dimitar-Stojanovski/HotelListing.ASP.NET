using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.DTOs.Users;
using HotelListing.API.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            bool isValidUser = false;

            try
            {
                var user = await userManager.FindByEmailAsync(loginDto.Email);
                isValidUser = await userManager.CheckPasswordAsync(user, loginDto.Password);
                return isValidUser;
            }
            catch (Exception)
            {

                throw;
            }
            return isValidUser;
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var result = await userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
            
            return result.Errors;
        }

   
    }
}
