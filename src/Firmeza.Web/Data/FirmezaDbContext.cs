using Firmeza.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Firmeza.Web.Data;

public class FirmezaDbContext(DbContextOptions<FirmezaDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasIndex(product => product.Code)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(customer => new { customer.DocumentType, customer.DocumentNumber })
            .IsUnique();

        modelBuilder.Entity<Sale>()
            .HasIndex(sale => sale.SaleNumber)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(product => product.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(product => product.CurrentStock)
            .HasPrecision(18, 3);

        modelBuilder.Entity<Product>()
            .Property(product => product.MinimumStock)
            .HasPrecision(18, 3);

        modelBuilder.Entity<SaleDetail>()
            .Property(detail => detail.Quantity)
            .HasPrecision(18, 3);

        modelBuilder.Entity<SaleDetail>()
            .Property(detail => detail.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SaleDetail>()
            .Property(detail => detail.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(sale => sale.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(sale => sale.Tax)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(sale => sale.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Sale>()
            .Property(sale => sale.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        modelBuilder.Entity<ProductCategory>()
            .HasMany(category => category.Products)
            .WithOne(product => product.ProductCategory)
            .HasForeignKey(product => product.ProductCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasMany(customer => customer.Sales)
            .WithOne(sale => sale.Customer)
            .HasForeignKey(sale => sale.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Sale>()
            .HasMany(sale => sale.Details)
            .WithOne(detail => detail.Sale)
            .HasForeignKey(detail => detail.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(product => product.SaleDetails)
            .WithOne(detail => detail.Product)
            .HasForeignKey(detail => detail.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(user => user.Customer)
            .WithMany()
            .HasForeignKey(user => user.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
