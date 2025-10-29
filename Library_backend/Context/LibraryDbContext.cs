using Library_backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library_backend.Context
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) //Configuring Connection string and options from program.cs using Dependency Injection
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)  //this is to override the class in IdentityDbContext which further inherits DBContext in IdentityDBContext
        {
            base.OnModelCreating(builder);                             //it is  used to create identity tables roles in DB

            builder.Entity<Author>()
                   .HasMany(a => a.Books)
                   .WithOne(b => b.Author)
                   .HasForeignKey(b => b.AuthorId);                  //Defining the relation between two tables Author and Books
                                                                     // Here an Author Entity has Many Connection With Books and adds AuthorId from Books as Foreign Key.
            builder.Entity<Book>(entity =>
            {
                entity.Property(b => b.BookTitle)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(b => b.ISBN)
                      .IsRequired()
                      .HasMaxLength(13);                              // Assuming ISBN-13 format
            });

        }


        public DbSet<Book> Books { get; set; } //This is used to configure the DbContext Class for the Author Entity.
        public DbSet<Author> Authors { get; set; }      //This is used to configure the DbContext Class for the Author Entity.

    }
}
