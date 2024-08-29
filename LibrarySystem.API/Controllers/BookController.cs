using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Authorize]
    [Route("api/buku")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookRepository _bookRepository;

        public BookController(IBookService bookService, IBookRepository bookRepository)
        {
            _bookService = bookService;
            _bookRepository = bookRepository;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [Authorize(Roles = "Librarian")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound("Buku tidak ditemukan");
            }
            return Ok(book);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Data buku tidak valid");
            }

            var result = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.IdBook }, result);
        
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || book.IdBook != id)
            {
                return BadRequest("Data buku tidak valid");
            }

            var result = await _bookService.UpdateBookAsync(book, id);
            if (result == "Buku tidak ditemukan")
            {
                return NotFound(result);
            }

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (result == "Buku tidak ditemukan")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooksAsync([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Masukan judul buku");
                
            }

            var books = await _bookRepository.SearchBooksAsync(title);

            if (books == null || !books.Any())
            {
                return NotFound("Buku tidak ditemukan");
            }

            return Ok(books);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Book>>> FilterBooksAsync([FromQuery] BookSearchCriteria criteria)
        {
            if (criteria == null)
            {
                return BadRequest("masukan filternya");
            }

            var books = await _bookRepository.FilterBooksAsync(criteria);

            if (books == null || !books.Any())
            {
                return NotFound("No books found");
            }

            return Ok(books);
        }

    }
}
