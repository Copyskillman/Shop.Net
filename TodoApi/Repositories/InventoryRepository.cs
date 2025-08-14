using Microsoft.EntityFrameworkCore;
using TodoApi.API.Data;
using TodoApi.API.Models.Entities;
using TodoApi.API.Repositories.Interfaces;

namespace TodoApi.API.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ShopDbContext _context;

        public InventoryRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories
                .ToListAsync();
        }

        public async Task<Inventory?> GetByProductIdAsync(int productId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == productId);
        }

        public async Task<IEnumerable<Inventory>> GetLowStockAsync()
        {
            return await _context.Inventories
                .Where(i => i.Quantity <= i.MinStock)
                .ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetExpiringSoonAsync(int days = 7)
        {
            var expiryDate = DateTime.Now.AddDays(days);
            
            return await _context.Inventories
                .Where(i => i.ExpiryDate.HasValue && i.ExpiryDate <= expiryDate)
                .ToListAsync();
        }

        public async Task<Inventory> UpdateStockAsync(int productId, decimal quantity, string movementType, string reference)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var inventory = await GetByProductIdAsync(productId);
                if (inventory == null)
                {
                    // สร้าง inventory ใหม่ถ้ายังไม่มี
                    inventory = new Inventory
                    {
                        ProductId = productId,
                        Quantity = 0,
                        MinStock = 0
                    };
                    _context.Inventories.Add(inventory);
                }

                // อัพเดทจำนวนสต็อก
                if (movementType == "in")
                    inventory.Quantity += quantity;
                else if (movementType == "out" || movementType == "sale")
                    inventory.Quantity -= quantity;
                else // adjustment
                    inventory.Quantity = quantity;

                // บันทึก Stock Movement
                var stockMovement = new StockMovement
                {
                    ProductId = productId,
                    MovementType = movementType,
                    Quantity = quantity,
                    ReferenceType = movementType == "sale" ? "sale" : 
                                   movementType == "out" ? "sale" : 
                                   movementType == "in" ? "purchase" : "adjustment",
                    Notes = reference,
                    CreatedAt = DateTime.Now
                };
                _context.StockMovements.Add(stockMovement);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return inventory;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CheckStockAvailabilityAsync(int productId, decimal quantity)
        {
            var inventory = await GetByProductIdAsync(productId);
            return inventory != null && inventory.Quantity >= quantity;
        }
    }
}