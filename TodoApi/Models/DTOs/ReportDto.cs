namespace TodoApi.API.Models.DTOs
{
    public class MonthlySalesReportDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalSales { get; set; }
        public int TransactionCount { get; set; }
        public decimal AverageTransaction { get; set; }
        public decimal GrowthRate { get; set; }
        public List<DailySalesDto> DailySales { get; set; } = new();
    }

    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class DashboardSummaryDto
    {
        public decimal TodaySales { get; set; }
        public decimal ThisMonthSales { get; set; }
        public int TodayTransactions { get; set; }
        public int LowStockCount { get; set; }
        public int ExpiringProductsCount { get; set; }
        public List<RecentSaleDto> RecentSales { get; set; } = new();
        public List<SalesChartDto> SalesChart { get; set; } = new();
    }

    public class RecentSaleDto
    {
        public string SaleNo { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class SalesChartDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Label { get; set; } = string.Empty;
    }

    public class AlertSummaryDto
    {
        public List<LowStockAlertDto> LowStockAlerts { get; set; } = new();
        public List<ExpiryAlertDto> ExpiryAlerts { get; set; } = new();
        public int TotalAlerts { get; set; }
    }

    public class ProfitAnalysisDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal ProfitMargin { get; set; }
        public List<ProductProfitDto> ProductProfits { get; set; } = new();
    }

    public class ProductProfitDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitMargin { get; set; }
    }
}
