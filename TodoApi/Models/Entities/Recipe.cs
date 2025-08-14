using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.API.Models.Entities
{
    [Table("recipes")]
    public class Recipe
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("ingredient_product_id")]
        public int IngredientProductId { get; set; }

        [Column("quantity_needed", TypeName = "decimal(10,3)")]
        public decimal QuantityNeeded { get; set; }

        [Column("unit")]
        [MaxLength(20)]
        public string Unit { get; set; } = "ml";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Product? Product { get; set; }
        public virtual Product? IngredientProduct { get; set; }
    }
}