using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IUserService
    {
        Task <string> AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task <string> UpdateUserAsync(User user, int id);
        Task <string> DeleteUserAsync(int id);
    }
}
