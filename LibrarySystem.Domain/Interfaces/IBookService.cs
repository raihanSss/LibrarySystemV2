using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IBookService
    {
        Task <string> AddBookAsync(Book book);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<string> UpdateBookAsync(Book book, int id);
        Task<string> DeleteBookAsync(int id);

       
    }
}
