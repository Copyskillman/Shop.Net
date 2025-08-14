using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.API.Models.Enums;

namespace TodoApi.API.Models.Entities
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("barcode")]
        [MaxLength(50)]
        public string? Barcode { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column("cost", TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        [Column("unit")]
        [MaxLength(20)]
        public string Unit { get; set; } = "piece";

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("has_expiry")]
        public bool HasExpiry { get; set; } = false;

        [Column("is_recipe_based")]
        public bool IsRecipeBased { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("inventory")]
    public class Inventory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("min_stock", TypeName = "decimal(10,2)")]
        public decimal MinStock { get; set; }

        [Column("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("batch_no")]
        [MaxLength(50)]
        public string? BatchNo { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("sales")]
    public class Sale
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("sale_no")]
        [MaxLength(50)]
        public string SaleNo { get; set; } = string.Empty;

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column("discount_amount", TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; }

        [Column("net_amount", TypeName = "decimal(10,2)")]
        public decimal NetAmount { get; set; }

        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Column("sale_date")]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Column("cashier_name")]
        [MaxLength(100)]
        public string? CashierName { get; set; }

        public virtual List<SaleItem> SaleItems { get; set; } = new();
    }

    [Table("sale_items")]
    public class SaleItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("product_name")]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("unit_price", TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
    }


}