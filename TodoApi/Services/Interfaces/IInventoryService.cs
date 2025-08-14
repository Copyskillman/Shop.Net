using TodoApi.API.Models.DTOs;

namespace TodoApi.API.Services
{
    public interface IInventoryService
    {
        Task<List<InventoryStatusDto>> GetInventoryStatusAsync();
        Task<List<LowStockAlertDto>> GetLowStockAlertsAsync();
        Task<List<ExpiryAlertDto>> GetExpiryAlertsAsync(int days = 7);
        Task<bool> UpdateStockAsync(int productId, decimal quantity, string movementType, string reference);
        Task<bool> ProcessRecipeStockAsync(int recipeProductId, decimal quantity);
    }
}