using Microsoft.EntityFrameworkCore;
using BooksAndQuotesApplication.Models;

namespace BooksAndQuotesApplication.Data;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().ToTable("Books");
        modelBuilder.Entity<Quote>().ToTable("Quotes");
    }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Quote> Quotes => Set<Quote>();
}
