using Microsoft.AspNetCore.Mvc;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Services;

namespace TodoApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// รายงานยอดขายรายวัน
        /// </summary>
        [HttpGet("daily-sales")]
        public async Task<ActionResult<DailySalesReportDto>> GetDailySalesReport([FromQuery] DateTime? date = null)
        {
            try
            {
                var reportDate = date ?? DateTime.Now.Date;
                var report = await _reportService.GetDailySalesReportAsync(reportDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// รายงานสินค้าขายดี
        /// </summary>
        [HttpGet("top-selling")]
        public async Task<ActionResult<List<TopSellingProductDto>>> GetTopSellingProducts(
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int limit = 10)
        {
            try
            {
                var start = startDate ?? DateTime.Now.AddDays(-30);
                var end = endDate ?? DateTime.Now;
                
                var report = await _reportService.GetTopSellingProductsAsync(start, end, limit);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }
    }
}