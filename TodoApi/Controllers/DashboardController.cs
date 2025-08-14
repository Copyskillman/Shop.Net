using Microsoft.AspNetCore.Mvc;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Services;

namespace TodoApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IInventoryService _inventoryService;

        public DashboardController(IReportService reportService, IInventoryService inventoryService)
        {
            _reportService = reportService;
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// ดึงข้อมูลสรุปสำหรับ Dashboard
        /// </summary>
        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboardSummary()
        {
            try
            {
                var summary = await _reportService.GetDashboardSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ดึงการแจ้งเตือนทั้งหมด
        /// </summary>
        [HttpGet("alerts")]
        public async Task<ActionResult<AlertSummaryDto>> GetAlerts()
        {
            try
            {
                var lowStockAlerts = await _inventoryService.GetLowStockAlertsAsync();
                var expiryAlerts = await _inventoryService.GetExpiryAlertsAsync();

                var alertSummary = new AlertSummaryDto
                {
                    LowStockAlerts = lowStockAlerts,
                    ExpiryAlerts = expiryAlerts,
                    TotalAlerts = lowStockAlerts.Count + expiryAlerts.Count
                };

                return Ok(alertSummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ดึงข้อมูลกราฟยอดขาย
        /// </summary>
        [HttpGet("sales-chart")]
        public async Task<ActionResult<List<SalesChartDto>>> GetSalesChart(
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string period = "daily")
        {
            try
            {
                var start = startDate ?? DateTime.Now.AddDays(-30);
                var end = endDate ?? DateTime.Now;

                var chartData = await _reportService.GetSalesChartDataAsync(start, end, period);
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }
    }
}
