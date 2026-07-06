using Library_backend.Context;
using Library_backend.DTO;
using Library_backend.Models;
using Library_backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library_UnitTests
{
    public class BookRepositoryTests
    {
        private LibraryDbContext _Context;
        private BookRepository _Repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "BookRepoTestDb")
                .Options;

            _Context = new LibraryDbContext(options);
            _Context.Database.EnsureDeleted(); // Ensure fresh DB for each test
            _Context.Database.EnsureCreated();

            _Repo = new BookRepository(_Context);
        }

        [TearDown]
        public void TearDown()
        {
            _Context.Dispose();
        }

        [Test]
        public async Task GetAllBooksAsync_WhenNoBooksPresent()
        {
            var result = await _Repo.GetAllBooksAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllBooksAsync_WhenBooksArePresent()
        {
            _Context.Books.Add(new Book { BookTitle = "Book 1", PublicationYear = 2021, AuthorId = 101, ISBN = "ABC123", Description = "This is a book.", BookUrl = "WWW.book1.read" });
            _Context.Books.Add(new Book { BookTitle = "Book 2", PublicationYear = 2022, AuthorId = 102, ISBN = "ABC123", Description = "This is a book.", BookUrl = "WWW.book2.read" });
            _Context.SaveChanges();

            var result = await _Repo.GetAllBooksAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetBookById_WhenBookNotFound()
        {
            var result = await _Repo.GetBookByIdAsync(99);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateBookAsync_AddsBook()
        {
            var newBook = new BookDTO { BookTitle = "New Book", Description = "New Desc", AuthorId = 3, ISBN = "ABC123732468s", PublicationYear = 2023, BookUrl = "WWW.newbook.read" };

            var createdBook = await _Repo.CreateBookAsync(newBook);

            Assert.That(createdBook.BookId, Is.GreaterThan(0));
            Assert.That(_Context.Books.Count(), Is.EqualTo(1));
            Assert.That(_Context.Books.Any(b => b.BookTitle == "New Book"), Is.True);
        }

        [Test]
        public async Task UpdateBookAsync_UpdatesExistingBook()
        {
            _Context.Books.Add(new Book { BookTitle = "OLD TITLE", Description = "Old Desc", AuthorId = 5, ISBN = "123456ABC", PublicationYear = 2021, BookUrl = "WWW.newbook.read" });
            await _Context.SaveChangesAsync();

            var updatedBookDto = new BookDTO { BookTitle = "Updated Title", Description = "Updated Desc", AuthorId = 5, ISBN = "123456ABC", PublicationYear = 2022, BookUrl = "WWW.updatedbook.read" };

            var result = await _Repo.UpdateBookAsync(1, updatedBookDto);

            Assert.That(result, Is.True);
            var updatedBook = _Context.Books.Find(1);
            Assert.That(updatedBook.BookTitle, Is.EqualTo("Updated Title"));
        }

        [Test]
        public async Task UpdateBookAsync_ReturnsFalse_WhenNotFound()
        {
            _Context.Books.Add(new Book { BookTitle = "Book 2", PublicationYear = 2022, AuthorId = 102, ISBN = "AAAAA", Description = "This is a book.", BookUrl = "WWW.book2.read" });
            await _Context.SaveChangesAsync();

            var bookDto = new BookDTO { BookTitle = "Doesn't matter", Description = "N/A", AuthorId = 1, ISBN = "123456ABC", PublicationYear = 2023, BookUrl = "wrong url" };

            var result = await _Repo.UpdateBookAsync(999, bookDto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteBookAsync_RemovesBook()
        {
            _Context.Books.Add(new Book { BookTitle = "Book 1", PublicationYear = 2021, AuthorId = 101, ISBN = "ABC123", Description = "This is a book.", BookUrl = "WWW.newbook.read" });
            _Context.Books.Add(new Book { BookTitle = "Book 2", PublicationYear = 2022, AuthorId = 102, ISBN = "ABC123", Description = "This is a book.", BookUrl = "www.book2.read" });
            await _Context.SaveChangesAsync();

            var result = await _Repo.DeleteBookAsync(1);

            Assert.That(result, Is.True);
            Assert.That(_Context.Books.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteBookAsync_ReturnsFalse_WhenNotFound()
        {
            _Context.Books.Add(new Book { BookTitle = "Book 1", PublicationYear = 2021, AuthorId = 101, ISBN = "ABC123", Description = "This is a book.", BookUrl = "WWW.book1.read" });
            await _Context.SaveChangesAsync();

            var result = await _Repo.DeleteBookAsync(999);

            Assert.That(result, Is.False);
        }
    }
}
