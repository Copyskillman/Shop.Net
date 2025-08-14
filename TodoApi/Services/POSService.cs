using TodoApi.API.Models.DTOs;
using TodoApi.API.Models.Entities;
using TodoApi.API.Repositories.Interfaces;
using TodoApi.API.Services;

namespace TodoApi.API.Services
{
    public class POSService : IPOSService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryService _inventoryService;

        public POSService(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IInventoryService inventoryService)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _inventoryService = inventoryService;
        }

        public async Task<SaleResponseDto> ProcessSaleAsync(CreateSaleDto saleDto)
        {
            try
            {
                // 1. ตรวจสอบความพร้อมของสต็อก
                var stockCheck = await ValidateStockAvailabilityAsync(saleDto.Items);
                if (!stockCheck)
                {
                    throw new InvalidOperationException("สต็อกสินค้าไม่เพียงพอ");
                }

                // 2. คำนวณยอดรวม
                var totalAmount = await CalculateTotalAsync(saleDto.Items, saleDto.DiscountAmount);

                // 3. สร้าง Sale Entity
                var sale = new Sale
                {
                    CustomerId = saleDto.CustomerId > 0 ? saleDto.CustomerId : null, // ถ้าไม่มี customer ให้เป็น null
                    TotalAmount = totalAmount,
                    DiscountAmount = saleDto.DiscountAmount,
                    NetAmount = totalAmount - saleDto.DiscountAmount,
                    PaymentMethod = saleDto.PaymentMethod,
                    CashierName = saleDto.CashierName,
                    SaleItems = new List<SaleItem>()
                };

                // 4. เพิ่ม Sale Items
                foreach (var itemDto in saleDto.Items)
                {
                    var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                    if (product == null) continue;

                    var saleItem = new SaleItem
                    {
                        ProductId = itemDto.ProductId,
                        ProductName = product.Name,
                        Quantity = itemDto.Quantity,
                        UnitPrice = product.Price,  // ใช้ราคาจากฐานข้อมูล
                        TotalAmount = itemDto.Quantity * product.Price
                    };

                    sale.SaleItems.Add(saleItem);
                }

                // 5. บันทึกการขาย
                var savedSale = await _saleRepository.AddAsync(sale);

                // 6. อัพเดทสต็อก (ลดสต็อกสินค้าที่ขาย)
                foreach (var item in sale.SaleItems)
                {
                    await _inventoryService.UpdateStockAsync(
                        item.ProductId, 
                        item.Quantity, // ส่งค่าบวก
                        "out",         // ใช้ "out" แทน "sale"
                        $"Sale: {savedSale.SaleNo}"
                    );

                    // ถ้าเป็นน้ำปั่น ต้องลดวัตถุดิบด้วย
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null && product.IsRecipeBased)
                    {
                        await _inventoryService.ProcessRecipeStockAsync(item.ProductId, item.Quantity);
                    }
                }

                // 7. Return ผลลัพธ์
                return new SaleResponseDto
                {
                    SaleId = savedSale.Id,
                    SaleNo = savedSale.SaleNo,
                    TotalAmount = savedSale.TotalAmount,
                    NetAmount = savedSale.NetAmount,
                    SaleDate = savedSale.SaleDate,
                    Success = true,
                    Message = "ขายสำเร็จ"
                };
            }
            catch (Exception ex)
            {
                return new SaleResponseDto
                {
                    Success = false,
                    Message = $"เกิดข้อผิดพลาด: {ex.Message}"
                };
            }
        }

        public async Task<bool> ValidateStockAvailabilityAsync(List<SaleItemDto> items)
        {
            foreach (var item in items)
            {
                var stockAvailable = await _inventoryRepository.CheckStockAvailabilityAsync(
                    item.ProductId, 
                    item.Quantity
                );
                
                if (!stockAvailable) return false;
            }
            return true;
        }

        public async Task<decimal> CalculateTotalAsync(List<SaleItemDto> items, decimal discountAmount = 0)
        {
            decimal total = 0;
            
            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    total += item.Quantity * product.Price;  // ใช้ราคาจากฐานข้อมูล
                }
            }
            
            return total;
        }

        public async Task<ProductInfoDto?> GetProductInfoAsync(string barcode)
        {
            var product = await _productRepository.GetByBarcodeAsync(barcode);
            if (product == null) return null;

            var inventory = await _inventoryRepository.GetByProductIdAsync(product.Id);

            return new ProductInfoDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Unit = product.Unit,
                AvailableStock = inventory?.Quantity ?? 0,
                IsRecipeBased = product.IsRecipeBased
            };
        }
    }
}