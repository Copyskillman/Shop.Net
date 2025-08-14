using Microsoft.EntityFrameworkCore;
using TodoApi.API.Data;
using TodoApi.API.Models.Entities;
using TodoApi.API.Repositories.Interfaces;

namespace TodoApi.API.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ShopDbContext _context;

        public SaleRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _context.Sales
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<Sale> AddAsync(Sale sale)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Generate Sale Number
                sale.SaleNo = await GenerateSaleNoAsync();
                sale.SaleDate = DateTime.Now;

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return sale;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<string> GenerateSaleNoAsync()
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Sales
                .CountAsync(s => s.SaleNo.StartsWith($"RC{today}")) + 1;
            
            return $"RC{today}{count:D4}";
        }

        public async Task<decimal> GetTodaySalesAsync()
        {
            var today = DateTime.Now.Date;
            return await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => s.NetAmount);
        }

        public async Task<IEnumerable<Sale>> GetTodayTransactionsAsync()
        {
            var today = DateTime.Now.Date;
            return await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }
    }
}