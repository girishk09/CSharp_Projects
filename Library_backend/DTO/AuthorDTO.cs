namespace Library_backend.DTO
{
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        public required string AuthorName { get; set; }
        public string? AuthorBiography { get; set; }
    }
}
