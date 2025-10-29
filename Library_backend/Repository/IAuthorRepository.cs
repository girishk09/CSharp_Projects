using Library_backend.DTO;
using Library_backend.Models;

namespace Library_backend.Repository
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync();
        Task<AuthorDTO> GetAuthorByIdAsync(int id);

        Task<AuthorDTO> CreateAuthorAsync(AuthorDTO authorDto);

        Task<List<Author>> SearchAuthorsAsync(string query);

        Task<bool> UpdateAuthorAsync(int id, AuthorDTO authorDto);

        Task<bool> DeleteAuthorAsync(int id);
    }
}
