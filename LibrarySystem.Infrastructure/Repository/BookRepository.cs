using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibrarysystemDbContext _context;

        public BookRepository(LibrarysystemDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<object>> SearchBooksAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Enumerable.Empty<object>();
            }

            var lower = title.ToLower();

            var books = await _context.Set<Book>()
                .Where(b => b.Title != null && b.Title.ToLower().Contains(lower))
                .Select(b => new
                {
                    category = b.Category,
                    title = b.Title,
                    isbn = b.Isbn,
                    author = b.Author,
                })
                .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<Book>> FilterBooksAsync(BookSearchCriteria criteria)
        {
            if (criteria == null)
            {
                return Enumerable.Empty<Book>();
            }

            var query = _context.Books.AsQueryable();

            
            query = query.Where(b =>
            (!string.IsNullOrEmpty(criteria.Title) && b.Title.Contains(criteria.Title)) ||
            (!string.IsNullOrEmpty(criteria.Isbn) && b.Isbn.Contains(criteria.Isbn)) ||
            (!string.IsNullOrEmpty(criteria.Author) && b.Author.Contains(criteria.Author)) ||
            (!string.IsNullOrEmpty(criteria.Category) && b.Category.Contains(criteria.Category))

            );
            
            return await query.ToListAsync();
        }

    }
}
