using Library_backend.DTO;
using Library_backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Get all books

        [HttpGet("getall-books")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            if(books == null || !books.Any())
            {
                return NotFound(new { message = "No books found."});
            }
            return Ok(books);
        }

        // Get book by id
        [HttpGet("get-book/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if(book == null)
            {
                return NotFound(new { status = "Not Found", message = $"Book with ID {id} not found."});
            }
            return Ok(book);
        }

        // Add new book
        [HttpPost("add-book")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook(int id, BookDTO bookDto)
        {
            var createdBook = await _bookRepository.CreateBookAsync(bookDto);
            return CreatedAtAction("GetBookById", new { id = createdBook.BookId }, createdBook);
        }

        // Update book
        [HttpPut("update-book/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, BookDTO bookDto)
        {
            var result = await _bookRepository.UpdateBookAsync(id, bookDto);
            if(!result)
            {
                return NotFound(new { status = "Not Found", message = $"Book with ID {id} not found."});
            }
            return Ok(new { status = "Success", message = "Book updated successfully."});
        }

        // Delete book
        [HttpDelete("delete-book/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookRepository.DeleteBookAsync(id);
            if(!result)
            {
                return NotFound(new { status = "Not Found", message = $"Book with ID {id} not found."});
            }
            return Ok(new { status = "Success", message = "Book deleted successfully."});
        }

        // Search books
        [HttpGet("search-books")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SearchBooks(string? title, string? author, int? publishedYear, string? isbn)
        {
            var books = await _bookRepository.SearchBooksAsync(title, author, publishedYear, isbn);
            if(books == null || !books.Any())
            {
                return NotFound(new { status = "Not Found", message = "No books found matching the search criteria."});
            }
            return Ok(books);
        }
    }
}
