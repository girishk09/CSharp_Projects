using Library_backend.DTO;

namespace Library_backend.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO?> GetBookByIdAsync(int id);
        Task<BookDTO> CreateBookAsync(BookDTO bookDto);
        Task<bool> UpdateBookAsync(int id, BookDTO bookDto);
        Task<bool> DeleteBookAsync(int id);

        Task<IEnumerable<BookDTO>> SearchBooksAsync(string? title, string? author, int? publishedYear, string? isbn);
    }
}
