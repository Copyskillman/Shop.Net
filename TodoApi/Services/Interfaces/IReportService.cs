using TodoApi.API.Models.DTOs;

namespace TodoApi.API.Services
{
    public interface IReportService
    {
        Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime date);
        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int limit = 10);
        Task<MonthlySalesReportDto> GetMonthlySalesReportAsync(int year, int month);
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<List<SalesChartDto>> GetSalesChartDataAsync(DateTime startDate, DateTime endDate, string period = "daily");
        Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime startDate, DateTime endDate);
    }
}