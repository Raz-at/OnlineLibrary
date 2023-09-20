using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models;

namespace OnlineLibrary.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Admin> Admins { get; set; }        
        public DbSet<UserBook> userBooks { get; set; }  
        public DbSet<User> Users { get; set; }  

    }
}
