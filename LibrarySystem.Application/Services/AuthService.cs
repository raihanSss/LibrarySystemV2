using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<ResponseModel> RegisterAsync(RegisterModel model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

                if (roleResult.Succeeded)
                {
                    return new ResponseModel
                    {
                        Status = "Success",
                        Message = "User registered successfully."
                    };
                }
                else
                {
   
                    await _userManager.DeleteAsync(user);

                    return new ResponseModel
                    {
                        Status = "Error",
                        Message = "User registered but failed to assign role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description))
                    };
                }
            }

            return new ResponseModel
            {
                Status = "Error",
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return new LoginResponseDto { Status = "Error", Message = "Invalid login attempt." };
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new LoginResponseDto { Status = "Error", Message = "Invalid username or password." };
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddMinutes(10),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                string refreshToken;
                if (user.RefreshToken != null && user.RefreshTokenExpiryTime > DateTime.UtcNow)
                {
                   
                    refreshToken = user.RefreshToken;
                }
                else
                {
                   
                    refreshToken = await GenerateRefreshTokenAsync(user);
                }

                return new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    ExpiredOn = token.ValidTo,
                    Status = "Success",
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = userRoles.FirstOrDefault() 
                };
            }

            return new LoginResponseDto { Status = "Error", Message = "Invalid username or password." };
        }

        public async Task<AuthRespone> CreateRoleAsync(string rolename)
        {
            if (!await _roleManager.RoleExistsAsync(rolename))
            {
                await _roleManager.CreateAsync(new IdentityRole(rolename));
            }
            return new AuthRespone { Status = "Success", Message = "Role created successfully!" };
        }




        private async Task<string> GenerateRefreshTokenAsync(AppUser user)
        {
            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2); 
            await _userManager.UpdateAsync(user);
            return newRefreshToken;
        }


        public async Task<AuthRespone> LogoutAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);
            }
            return new AuthRespone
            {
                Message = "Success Logout",
                Status = "Success",
            };
        }


        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RefreshToken))
            {
                return new RefreshTokenResponseDto { Status = "Error", Message = "Invalid refresh token." };
            }

            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshToken == model.RefreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

            if (user == null)
            {
                return new RefreshTokenResponseDto { Status = "Error", Message = "Invalid or expired refresh token." };
            }

            // Generate akses token baru
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));

            var newToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // Optionally generate a new refresh token
            string newRefreshToken = await GenerateRefreshTokenAsync(user);

            return new RefreshTokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newToken),
                RefreshToken = newRefreshToken,
                ExpiredOn = newToken.ValidTo,
                Status = "Success"
            };
        }

    }
       
}
