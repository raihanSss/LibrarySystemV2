using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
            return "Data user berhasil ditambah";
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<string> UpdateUserAsync(User user, int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser != null)
            {
                existingUser.Fname = user.Fname;
                existingUser.Lname = user.Lname;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Librarycard = user.Librarycard;
                existingUser.Cardexp = user.Cardexp;
                existingUser.Notreturnbook = user.Notreturnbook;
                existingUser.Position = user.Position;
                existingUser.Penalty = user.Penalty;    

                await _userRepository.UpdateUserAsync(existingUser);
                return "Data user berhasil diubah";
            }
            return "User tidak ditemukan";
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteUserAsync(id);
                return "Data user berhasil dihapus";
            }
            return "User tidak ditemukan";
        }

        
    }
}
