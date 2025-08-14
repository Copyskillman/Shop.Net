namespace TodoApi.API.Models.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string? Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; } = "piece";
        public bool HasExpiry { get; set; } = false;
        public bool IsRecipeBased { get; set; } = false;
    }

    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; } = "piece";
    }

    public class CalculateTotalDto
    {
        public List<SaleItemDto> Items { get; set; } = new();
        public decimal DiscountAmount { get; set; } = 0;
    }

    public class AdjustStockDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string? Reason { get; set; }
    }

    public class DailySalesReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int TransactionCount { get; set; }
        public decimal AverageTransaction { get; set; }
        public List<PaymentMethodSummaryDto> PaymentMethods { get; set; } = new();
        public List<HourlySalesDto> HourlySales { get; set; } = new();
    }

    public class PaymentMethodSummaryDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Count { get; set; }
    }

    public class HourlySalesDto
    {
        public int Hour { get; set; }
        public decimal Amount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TransactionCount { get; set; }
    }
}