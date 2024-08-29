using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (result.Status == "Error")
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (result.Status == "Error")
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("role")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] string rolename)
        {
            var result = await _authService.CreateRoleAsync(rolename);
            if (result.Status == "Error")
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel model)
        {
            var response = await _authService.RefreshTokenAsync(model);
            if (response.Status == "Success")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
