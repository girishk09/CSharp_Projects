
using Library_backend.Context;
using Library_backend.DTO;
using Library_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_backend.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    BookTitle = b.BookTitle,
                    Description = b.Description,
                    ISBN = b.ISBN,
                    PublicationYear = b.PublicationYear,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.AuthorName,
                    BookUrl = b.BookUrl,
                }).ToListAsync();
        }

        public async Task<BookDTO?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return null;

            return new BookDTO
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                Description = book.Description,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.AuthorName,
                BookUrl = book.BookUrl,
            };
        }

        public async Task<BookDTO> CreateBookAsync(BookDTO bookDto)
        {
            var book = new Book
            {
                BookTitle = bookDto.BookTitle,
                Description = bookDto.Description,
                ISBN = bookDto.ISBN,
                PublicationYear = bookDto.PublicationYear,
                AuthorId = bookDto.AuthorId,
                BookUrl = bookDto.BookUrl,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var author = await _context.Authors.FindAsync(bookDto.AuthorId);
            bookDto.BookId = book.BookId;
            bookDto.AuthorName = author?.AuthorName ?? string.Empty;

            return bookDto;
        }

        public async Task<bool> UpdateBookAsync(int id, BookDTO bookDto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            // update book fields
            book.BookTitle = bookDto.BookTitle;
            book.Description = bookDto.Description;
            book.ISBN = bookDto.ISBN;
            book.PublicationYear = bookDto.PublicationYear;
            book.AuthorId = bookDto.AuthorId;



            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookDTO>> SearchBooksAsync(string? title, string? author, int? publishedYear, string? isbn)
        {
            var booksQuery = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                booksQuery = booksQuery.Where(b => b.BookTitle.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                booksQuery = booksQuery.Where(b => b.Author.AuthorName.Contains(author));

            if (!string.IsNullOrWhiteSpace(isbn))
                booksQuery = booksQuery.Where(b => b.ISBN.Contains(isbn));

            if (publishedYear.HasValue && publishedYear > 0)
                booksQuery = booksQuery.Where(b => b.PublicationYear == publishedYear.Value);

            return await booksQuery.Select(b => new BookDTO
            {
                BookId = b.BookId,
                BookTitle = b.BookTitle,
                Description = b.Description,
                ISBN = b.ISBN,
                PublicationYear = b.PublicationYear,
                AuthorId = b.AuthorId,
                AuthorName = b.Author.AuthorName,
                BookUrl = b.BookUrl,
            }).ToListAsync();
        }
    }
}
