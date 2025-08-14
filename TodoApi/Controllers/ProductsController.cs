using Microsoft.AspNetCore.Mvc;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Models.Entities;
using TodoApi.API.Repositories.Interfaces;

namespace TodoApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductsController(
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
        }

        /// <summary>
        /// ดึงสินค้าทั้งหมด
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาดในการดึงข้อมูลสินค้า", error = ex.Message });
            }
        }
        /// <summary>
        /// ดึงสินค้าตาม ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
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
        /// ดึงข้อมูลสินค้าจาก Barcode (สำหรับ POS)
        /// </summary>
        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<ProductInfoDto>> GetProductByBarcode(string barcode)
        {
            try
            {
                var product = await _productRepository.GetByBarcodeAsync(barcode);
                if (product == null)
                    return NotFound(new { message = "ไม่พบสินค้า" });

                var inventory = await _inventoryRepository.GetByProductIdAsync(product.Id);

                var productInfo = new ProductInfoDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Unit = product.Unit,
                    AvailableStock = inventory?.Quantity ?? 0,
                    IsRecipeBased = product.IsRecipeBased
                };

                return Ok(productInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// เพิ่มสินค้าใหม่
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId,
                    Barcode = productDto.Barcode,
                    Price = productDto.Price,
                    Cost = productDto.Cost,
                    Unit = productDto.Unit,
                    HasExpiry = productDto.HasExpiry,
                    IsRecipeBased = productDto.IsRecipeBased
                };

                var createdProduct = await _productRepository.AddAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาดในการเพิ่มสินค้า", error = ex.Message });
            }
        }

        /// <summary>
        /// อัพเดทสินค้า
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto productDto)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                    return NotFound(new { message = "ไม่พบสินค้า" });

                existingProduct.Name = productDto.Name;
                existingProduct.Description = productDto.Description;
                existingProduct.CategoryId = productDto.CategoryId;
                existingProduct.Price = productDto.Price;
                existingProduct.Cost = productDto.Cost;
                existingProduct.Unit = productDto.Unit;

                await _productRepository.UpdateAsync(existingProduct);
                return Ok(new { message = "อัพเดทสินค้าสำเร็จ" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }

        /// <summary>
        /// ลบสินค้า (Soft Delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productRepository.DeleteAsync(id);
                if (!result)
                    return NotFound(new { message = "ไม่พบสินค้า" });

                return Ok(new { message = "ลบสินค้าสำเร็จ" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาด", error = ex.Message });
            }
        }
    }
}