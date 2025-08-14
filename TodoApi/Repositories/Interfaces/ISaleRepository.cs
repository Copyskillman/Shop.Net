using TodoApi.API.Models.Entities;

namespace TodoApi.API.Repositories.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale?> GetByIdAsync(int id);
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Sale> AddAsync(Sale sale);
        Task<string> GenerateSaleNoAsync();
        Task<decimal> GetTodaySalesAsync();
        Task<IEnumerable<Sale>> GetTodayTransactionsAsync();
    }
}