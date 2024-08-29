using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User tidak ditemukan");
            }
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Data user tidak valid");
            }

            var result = await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.IdUser }, result);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || user.IdUser != id)
            {
                return BadRequest("Data user tidak valid");
            }

            var result = await _userService.UpdateUserAsync(user, id);
            if (result == "Buku tidak ditemukan")
            {
                return NotFound(result);
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result == "Buku tidak ditemukan")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
