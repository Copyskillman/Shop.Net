using TodoApi.API.Models.DTOs;

namespace TodoApi.API.Services
{
    public interface IPOSService
    {
        Task<SaleResponseDto> ProcessSaleAsync(CreateSaleDto saleDto);
        Task<bool> ValidateStockAvailabilityAsync(List<SaleItemDto> items);
        Task<decimal> CalculateTotalAsync(List<SaleItemDto> items, decimal discountAmount = 0);
        Task<ProductInfoDto?> GetProductInfoAsync(string barcode);
    }
}