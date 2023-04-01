using AspNetAngular.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetAngular.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }

    public DbSet<Note> Notes { get; set; }
}