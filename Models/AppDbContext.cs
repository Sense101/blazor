using Microsoft.EntityFrameworkCore;

namespace Blazor.Models;

public class AppDbContext : DbContext
{
    public DbSet<Basket> baskets { get; set; }
    public DbSet<BasketItem> items { get; set; }
    public DbSet<Product> products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=database.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(new Product { Id = 1, Name = "Product 1", Price = 2.5M, Description = "Description" });
        modelBuilder.Entity<Product>().HasData(new Product { Id = 2, Name = "Product 2", Price = 8.5M, Description = "Description" });
        modelBuilder.Entity<Product>().HasData(new Product { Id = 3, Name = "Product 3", Price = 6M, Description = "Description" });
        modelBuilder.Entity<Product>().HasData(new Product { Id = 4, Name = "Product 4", Price = 2.6M, Description = "Description" });

        modelBuilder.Entity<Basket>().HasData(new Basket { Id = 1 });

        // as far as I can see, no point seeding any of the other data because it won't be there at the start
    }
}