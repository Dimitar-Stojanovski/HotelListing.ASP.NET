using HotelListing.API.Contracts;
using HotelListing.API.Data.DTOs.Users;
using HotelListing.API.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ILogger<AccountController> _loger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> _loger)
        {
            this.authManager = authManager;
            this._loger = _loger;
        }

        //api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody]ApiUserDto apiUserDto)
        {
            _loger.LogInformation($"Registration attempt for {apiUserDto.Email}");

            try
            {
                var errors = await authManager.Register(apiUserDto);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return Ok();
            }
            catch (Exception ex)
            {

                _loger.LogError(ex, $"Something went wrong in the {nameof(Register)} -User Registration attempt  for {apiUserDto.Email}");
                return Problem($"Something went wrong in the {nameof(Register)}. Please Contact support.", statusCode:500);
            }

          
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult>LoginUser([FromBody]LoginDto loginDto)
        {
            _loger.LogInformation($"Login attempt for {loginDto.Email}");
            try
            {
                var authResponse = await authManager.Login(loginDto);
                if (authResponse == null)
                    return Unauthorized();
                return Ok(authResponse);
            }
            catch (Exception ex)
            {

                _loger.LogWarning(ex, $"User is constantly receiving warnings in {nameof(LoginUser)}, User {loginDto.Email}");
                return Problem($"User {nameof(LoginUser)} constantly returns", statusCode: 400);

                _loger.LogError(ex, $"The service carshes in {nameof(LoginUser)} for user {loginDto.Email}");
                return Problem($"The user {nameof(LoginUser)} receives status code", statusCode: 500);
                
            }
        }

        [HttpPost]
        [Route("refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto authResponseDto)
        {
            var authResponse = await authManager.VerifyRefreshToken(authResponseDto);
            if (authResponse == null)
                return Unauthorized();
            return Ok(authResponse);
        }

    }
}
