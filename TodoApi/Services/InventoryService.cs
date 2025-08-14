using Microsoft.EntityFrameworkCore;
using TodoApi.API.Data;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Repositories.Interfaces;
using TodoApi.API.Services;

namespace TodoApi.API.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly ShopDbContext _context;

        public InventoryService(
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            ShopDbContext context)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<List<InventoryStatusDto>> GetInventoryStatusAsync()
        {
            var inventories = await _inventoryRepository.GetAllAsync();
            var result = new List<InventoryStatusDto>();

            foreach (var inv in inventories)
            {
                var product = await _productRepository.GetByIdAsync(inv.ProductId);
                if (product == null) continue;

                result.Add(new InventoryStatusDto
                {
                    ProductId = inv.ProductId,
                    ProductName = product.Name,
                    CurrentStock = inv.Quantity,
                    MinStock = inv.MinStock,
                    Unit = product.Unit,
                    IsLowStock = inv.Quantity <= inv.MinStock,
                    ExpiryDate = inv.ExpiryDate,
                    BatchNo = inv.BatchNo
                });
            }

            return result;
        }

        public async Task<List<LowStockAlertDto>> GetLowStockAlertsAsync()
        {
            var lowStockItems = await _inventoryRepository.GetLowStockAsync();
            var alerts = new List<LowStockAlertDto>();

            foreach (var item in lowStockItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                alerts.Add(new LowStockAlertDto
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    CurrentStock = item.Quantity,
                    MinStock = item.MinStock,
                    Unit = product.Unit,
                    AlertLevel = item.Quantity == 0 ? "สินค้าหมด" : "สต็อกต่ำ"
                });
            }

            return alerts;
        }

        public async Task<List<ExpiryAlertDto>> GetExpiryAlertsAsync(int days = 7)
        {
            var expiringItems = await _inventoryRepository.GetExpiringSoonAsync(days);
            var alerts = new List<ExpiryAlertDto>();

            foreach (var item in expiringItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                var daysUntilExpiry = item.ExpiryDate?.Subtract(DateTime.Now).Days ?? 0;

                alerts.Add(new ExpiryAlertDto
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    ExpiryDate = item.ExpiryDate,
                    DaysUntilExpiry = daysUntilExpiry,
                    Quantity = item.Quantity,
                    BatchNo = item.BatchNo,
                    AlertLevel = daysUntilExpiry <= 0 ? "หมดอายุแล้ว" : 
                                daysUntilExpiry <= 2 ? "ใกล้หมดอายุ" : "เตือนล่วงหน้า"
                });
            }

            return alerts;
        }

        public async Task<bool> UpdateStockAsync(int productId, decimal quantity, string movementType, string reference)
        {
            try
            {
                await _inventoryRepository.UpdateStockAsync(productId, quantity, movementType, reference);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProcessRecipeStockAsync(int recipeProductId, decimal quantity)
        {
            try
            {
                // ดึงสูตรของสินค้า (น้ำปั่น)
                var recipes = await _context.Recipes
                    .Where(r => r.ProductId == recipeProductId)
                    .ToListAsync();

                // ลดสต็อกวัตถุดิบตามสูตร
                foreach (var recipe in recipes)
                {
                    var requiredQuantity = recipe.QuantityNeeded * quantity;
                    await UpdateStockAsync(
                        recipe.IngredientProductId, 
                        -requiredQuantity, 
                        "recipe_use",
                        $"Used for {recipeProductId} x{quantity}"
                    );
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}