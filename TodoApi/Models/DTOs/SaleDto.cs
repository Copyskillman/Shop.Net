using TodoApi.API.Models.Enums;

namespace TodoApi.API.Models.DTOs
{
    public class CreateSaleDto
    {
        public int? CustomerId { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
        public decimal DiscountAmount { get; set; } = 0;
        public PaymentMethod PaymentMethod { get; set; }
        public string? CashierName { get; set; }
    }

    public class SaleItemDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        // UnitPrice ถูกลบออก - ใช้ราคาจากฐานข้อมูลแทน
    }

    public class SaleResponseDto
    {
        public int SaleId { get; set; }
        public string SaleNo { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ProductInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal AvailableStock { get; set; }
        public bool IsRecipeBased { get; set; }
    }

    public class InventoryStatusDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool IsLowStock { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? BatchNo { get; set; }
    }

    public class LowStockAlertDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string AlertLevel { get; set; } = string.Empty;
    }

    public class ExpiryAlertDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public decimal Quantity { get; set; }
        public string? BatchNo { get; set; }
        public string AlertLevel { get; set; } = string.Empty;
    }
}