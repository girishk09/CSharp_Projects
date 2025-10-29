using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_backend.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Book title is required.")]
        public required string BookTitle { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        public required string ISBN { get; set; }

        [Required(ErrorMessage = "PublicationYear is required.")]
        public int PublicationYear { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required]
        public string BookUrl { get; set; }

        // Foreign key to Author
        [ForeignKey("Author")]
        public int AuthorId { get; set; }


        public Author? Author { get; set; }
    }
}
