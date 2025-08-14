using Microsoft.EntityFrameworkCore;
using TodoApi.API.Models.Entities;

namespace TodoApi.API.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }

        // DbSets for all entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Cost).HasPrecision(10, 2);
                entity.Property(e => e.IsActive).HasConversion<bool>();
                entity.Property(e => e.HasExpiry).HasConversion<bool>();
                entity.Property(e => e.IsRecipeBased).HasConversion<bool>();
                entity.HasOne<Category>().WithMany().HasForeignKey(e => e.CategoryId);
            });

            // Sale Configuration
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SaleNo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
                entity.Property(e => e.NetAmount).HasPrecision(10, 2);
                entity.HasMany(e => e.SaleItems).WithOne().HasForeignKey(si => si.SaleId);
            });

            // SaleItem Configuration
            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(10, 2);
                entity.Property(e => e.UnitPrice).HasPrecision(10, 2);
                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
            });

            // Inventory Configuration
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasPrecision(10, 2);
                entity.HasOne<Product>().WithMany().HasForeignKey(e => e.ProductId);
            });

            // Configure Enums
            modelBuilder.Entity<Sale>()
                .Property(e => e.PaymentMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(e => e.OrderStatus)
                .HasConversion<string>();
        }
    }
}