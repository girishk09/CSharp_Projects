using AutoMapper;
using Library_backend.Context;
using Library_backend.DTO;
using Library_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_backend.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public AuthorRepository(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors.Include(a => a.Books).ToListAsync();
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }

        public async Task<AuthorDTO?> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            return author == null ? null : _mapper.Map<AuthorDTO>(author);
        }

        public async Task<AuthorDTO> CreateAuthorAsync(AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return _mapper.Map<AuthorDTO>(author);
        }

        public async Task<bool> UpdateAuthorAsync(int id, AuthorDTO authorDto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            // Map updated values from DTO to existing entity
            _mapper.Map(authorDto, author);


            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Author>> SearchAuthorsAsync(string query)
        {
            var authors = await _context.Authors.ToListAsync();

            if (!string.IsNullOrEmpty(query))
            {
                authors = authors
                    .Where(a => a.AuthorName.Contains(query, StringComparison.OrdinalIgnoreCase)
                             || a.AuthorBiography.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return authors;
        }
    }
}
