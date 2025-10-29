using Microsoft.AspNetCore.Identity;

namespace Library_backend.Context
{
    public class ApplicationUser : IdentityUser
    {
        public string? Code { get; set; }
    }
}
