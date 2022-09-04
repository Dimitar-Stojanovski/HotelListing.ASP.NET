using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Data.DTOs.Users;
using HotelListing.API.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration; //goes to the configuration file i.e app.settings.json
        private ApiUser _user;

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

      

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {

            _user = await userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (_user ==null || isValidUser==false)
            {
                return null;
            }
            var token = await GenerateToken();
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
            
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(_user, "User");
            }
            
            return result.Errors;
        }

        public async Task<string> GenerateToken()
        {
            var jwtKey = configuration["JwtSettings:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(_user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                //JwtRegisteredClaimNames.Sub represents the subject to whom the token has been issued. 
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id)
            }
            .Union(userClaims).Union(roleClaims);

            //Creating token
            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);

            var newRefreshToken = await userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            var result = await userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;

        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
           var _jwtSecurityHandler = new JwtSecurityTokenHandler();
           var _tokenContent = _jwtSecurityHandler.ReadJwtToken(request.Token);
           var _userName = _tokenContent.Claims.ToList().FirstOrDefault(q=>q.Type==JwtRegisteredClaimNames.Email)?.Value;
           _user= await userManager.FindByNameAsync(_userName);
            if (_user==null || _user.Id!=request.UserId)
            {
                return null;
            }

            var isValidRefreshToken = await userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);
            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await userManager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
