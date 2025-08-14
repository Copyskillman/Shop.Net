using TodoApi.API.Models.Entities;

namespace TodoApi.API.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> GetByProductIdAsync(int productId);
        Task<IEnumerable<Inventory>> GetLowStockAsync();
        Task<IEnumerable<Inventory>> GetExpiringSoonAsync(int days = 7);
        Task<Inventory> UpdateStockAsync(int productId, decimal quantity, string movementType, string reference);
        Task<bool> CheckStockAvailabilityAsync(int productId, decimal quantity);
    }
}