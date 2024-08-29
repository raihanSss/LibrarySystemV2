using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<string> AddBookAsync(Book book)
        {
            await _bookRepository.AddBookAsync(book);
            return "Data buku berhasil ditambah";
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetBookByIdAsync(id);
        }

        public async Task<string> UpdateBookAsync(Book book, int id)
        {
            var existingBook = await _bookRepository.GetBookByIdAsync(id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Isbn = book.Isbn;
                existingBook.Category = book.Category;
                existingBook.Description = book.Description;
                existingBook.Location = book.Location;
                existingBook.Publisher = book.Publisher;
                existingBook.Status = book.Status;
                existingBook.Availablebook = book.Availablebook;
                existingBook.Language = book.Language;

                await _bookRepository.UpdateBookAsync(existingBook);
                return "Data buku berhasil diubah";
            }
            return "Buku tidak ditemukan";
        }

        public async Task<string> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book != null)
            {
                await _bookRepository.DeleteBookAsync(id);
                return "Data buku berhasil dihapus";
            }
            return "Buku tidak ditemukan";
        }
    }
}
