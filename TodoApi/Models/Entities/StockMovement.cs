using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.API.Models.Enums;

namespace TodoApi.API.Models.Entities
{
    [Table("stock_movements")]
    public class StockMovement
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("movement_type")]
        [MaxLength(20)]
        public string MovementType { get; set; } = string.Empty; // in, out, adjustment

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("reference_type")]
        [MaxLength(50)]
        public string ReferenceType { get; set; } = string.Empty; // sale, purchase, adjustment

        [Column("reference_id")]
        public int? ReferenceId { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Product? Product { get; set; }
    }

    [Table("customers")]
    public class Customer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Column("phone")]
        [MaxLength(20)]
        public string? Phone { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("is_member")]
        public bool IsMember { get; set; } = false;

        [Column("points")]
        public int Points { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("order_no")]
        [MaxLength(50)]
        public string OrderNo { get; set; } = string.Empty;

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column("order_status")]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        [Column("order_type")]
        [MaxLength(20)]
        public string OrderType { get; set; } = "takeaway";

        [Column("scheduled_time")]
        public DateTime? ScheduledTime { get; set; }

        [Column("customer_notes")]
        public string? CustomerNotes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual List<OrderItem> OrderItems { get; set; } = new();
    }

    [Table("order_items")]
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("product_name")]
        [MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Column("quantity", TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }

        [Column("unit_price", TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column("special_instructions")]
        public string? SpecialInstructions { get; set; }

        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}