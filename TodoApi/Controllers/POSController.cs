using Microsoft.AspNetCore.Mvc;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Services;
using TodoApi.API.Repositories.Interfaces;

namespace TodoApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class POSController : ControllerBase
    {
        private readonly IPOSService _posService;
        private readonly ISaleRepository _saleRepository;

        public POSController(IPOSService posService, ISaleRepository saleRepository)
        {
            _posService = posService;
            _saleRepository = saleRepository;
        }

        /// <summary>
        /// ประมวลผลการขาย
        /// </summary>
        [HttpPost("sale")]
        public async Task<ActionResult<SaleResponseDto>> ProcessSale(CreateSaleDto saleDto)
        {
            try
            {
                var result = await _posService.ProcessSaleAsync(saleDto);
                
                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = "เกิดข้อผิดพลาดในการประมวลผลการขาย", 
                    error = ex.Message 
                });
            }
        }

        /// <summary>
        /// ตรวจสอบข้อมูลสินค้าจาก Barcode
        /// </summary>
        [HttpGet("product/{barcode}")]
        public async Task<ActionResult<ProductInfoDto>> GetProductInfo(string barcode)
        {
            try
            {
                var product = await _posService.GetProductInfoAsync(barcode);
                if (product == null)
                    return NotFound(new { message = "ไม่พบสินค้า" });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ตรวจสอบความพร้อมของสต็อก
        /// </summary>
        [HttpPost("validate-stock")]
        public async Task<ActionResult<bool>> ValidateStock(List<SaleItemDto> items)
        {
            try
            {
                var isAvailable = await _posService.ValidateStockAvailabilityAsync(items);
                return Ok(new { available = isAvailable });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// คำนวณยอดรวม
        /// </summary>
        [HttpPost("calculate-total")]
        public async Task<ActionResult<decimal>> CalculateTotal(CalculateTotalDto dto)
        {
            try
            {
                var total = await _posService.CalculateTotalAsync(dto.Items, dto.DiscountAmount);
                return Ok(new { total = total });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ดึงยอดขายวันนี้
        /// </summary>
        [HttpGet("today-sales")]
        public async Task<ActionResult> GetTodaySales()
        {
            try
            {
                var todaySales = await _saleRepository.GetTodaySalesAsync();
                var transactions = await _saleRepository.GetTodayTransactionsAsync();

                return Ok(new 
                { 
                    totalSales = todaySales,
                    transactionCount = transactions.Count(),
                    transactions = transactions.Take(10) // แสดง 10 รายการล่าสุด
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }
    }
}
