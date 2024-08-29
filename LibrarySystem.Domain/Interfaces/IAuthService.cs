using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel> RegisterAsync(RegisterModel registerModel);
        Task<LoginResponseDto> LoginAsync(LoginModel loginModel);

        Task<AuthRespone> CreateRoleAsync(string rolename);

        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestModel model);
    }
}
