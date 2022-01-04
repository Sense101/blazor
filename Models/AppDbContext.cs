using Microsoft.EntityFrameworkCore;

namespace Blazor.Models;

public class AppDbContext : DbContext
{
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(new Product { Id = 1, Name = "Product One" });
    }
}