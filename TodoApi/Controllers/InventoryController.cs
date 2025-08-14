using Microsoft.AspNetCore.Mvc;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Services;

namespace TodoApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// ดึงสถานะสต็อกทั้งหมด
        /// </summary>
        [HttpGet("status")]
        public async Task<ActionResult<List<InventoryStatusDto>>> GetInventoryStatus()
        {
            try
            {
                var status = await _inventoryService.GetInventoryStatusAsync();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ดึงการแจ้งเตือนสต็อกต่ำ
        /// </summary>
        [HttpGet("low-stock-alerts")]
        public async Task<ActionResult<List<LowStockAlertDto>>> GetLowStockAlerts()
        {
            try
            {
                var alerts = await _inventoryService.GetLowStockAlertsAsync();
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ดึงการแจ้งเตือนสินค้าใกล้หมดอายุ
        /// </summary>
        [HttpGet("expiry-alerts")]
        public async Task<ActionResult<List<ExpiryAlertDto>>> GetExpiryAlerts([FromQuery] int days = 7)
        {
            try
            {
                var alerts = await _inventoryService.GetExpiryAlertsAsync(days);
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ปรับสต็อกสินค้า
        /// </summary>
        [HttpPost("adjust-stock")]
        public async Task<IActionResult> AdjustStock(AdjustStockDto dto)
        {
            try
            {
                var result = await _inventoryService.UpdateStockAsync(
                    dto.ProductId, 
                    dto.Quantity, 
                    "adjustment", 
                    dto.Reason ?? "Manual adjustment"
                );

                if (!result)
                    return BadRequest(new { message = "ไม่สามารถปรับสต็อกได้" });

                return Ok(new { message = "ปรับสต็อกสำเร็จ" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }
    }
}