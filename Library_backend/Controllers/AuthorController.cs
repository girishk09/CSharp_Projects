using AutoMapper;
using Library_backend.DTO;
using Library_backend.Models;
using Library_backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorController> _logger;
        public AuthorController(IAuthorRepository repository, IMapper mapper, ILogger<AuthorController> logger)
        {
            _authorRepository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        // Get all authors
        [HttpGet("getAll-authors")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAllAuthorsAsync();
            return Ok(_mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        // Creating a new author    
        [HttpPost("add-author")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAuthor(AuthorDTO authorDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateAuthor");
                return BadRequest(ModelState);
            }
            var author = _mapper.Map<Author>(authorDto);
            var createdAuthor = await _authorRepository.CreateAuthorAsync(authorDto);
            _logger.LogInformation("Author created with ID: {AuthorId}", createdAuthor.AuthorId);
            return Ok(_mapper.Map<AuthorDTO>(createdAuthor));
        }

        // Get Author by Id

        [HttpGet("getAuthor/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Author with ID: {AuthorId} not found", id);
                return NotFound(new { message = "Author not found." });
            }
            return Ok(_mapper.Map<AuthorDTO>(author));
        }

        // Update Author
        [HttpPut("update-author/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorDTO authorDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for UpdateAuthor");
                return BadRequest(new { message = "Invalid input" });
            }
            var updatedAuthor = await _authorRepository.UpdateAuthorAsync(id, authorDto);
            if (!updatedAuthor)
            {
                _logger.LogWarning("Failed to update Author with ID: {AuthorId}", id);
                return NotFound(new { message = "Author not found." });
            }
            _logger.LogInformation("Author with ID: {AuthorId} updated successfully", id);
            return Ok(new { success = true, message = "Author updated successfully." });
        }

        // Delete Author
        [HttpDelete("delete-author/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var deleted = await _authorRepository.DeleteAuthorAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Failed to delete Author with ID: {AuthorId}", id);
                return NotFound(new { message = "Author not found." });
            }
            _logger.LogInformation("Author with ID: {AuthorId} deleted successfully", id);
            return Ok(new { success = true, message = "Author deleted successfully." });
        }

        // Search Authors
        [HttpGet("search-authors")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SearchAuthors(string query)
        {
            var authors = await _authorRepository.SearchAuthorsAsync(query);
            return Ok(authors);
        }

    }
}
