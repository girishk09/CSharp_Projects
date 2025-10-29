namespace Library_backend.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public required string BookTitle { get; set; }
        public required string Description { get; set; }
        public required string ISBN { get; set; }
        public required int PublicationYear { get; set; }
        public required int AuthorId { get; set; } // Foreign key to Author
        public string AuthorName { get; set; } = string.Empty;

        public required string BookUrl { get; set; }
    }
}
