using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Models;

namespace LibrarySystem.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);

        Task<IEnumerable<object>> SearchBooksAsync(string title);

        Task<IEnumerable<Book>> FilterBooksAsync(BookSearchCriteria criteria);


    }
}
