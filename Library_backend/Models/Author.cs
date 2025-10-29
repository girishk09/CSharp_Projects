using System.ComponentModel.DataAnnotations;

namespace Library_backend.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public string? AuthorName { get; set; }

        public string? AuthorBiography { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
