using Microsoft.EntityFrameworkCore;
using Ordigo.Server.Core.Data.Entities;

namespace Ordigo.Server.Core.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<TextNote> TextNotes { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
