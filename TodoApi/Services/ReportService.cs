using Microsoft.EntityFrameworkCore;
using TodoApi.API.Data;
using TodoApi.API.Models.DTOs;
using TodoApi.API.Models.Entities;

namespace TodoApi.API.Services
{
    public class ReportService : IReportService
    {
        private readonly ShopDbContext _context;

        public ReportService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var sales = await _context.Sales
                .Include(s => s.SaleItems)
                .Where(s => s.SaleDate >= startDate && s.SaleDate < endDate)
                .ToListAsync();

            var totalSales = sales.Sum(s => s.NetAmount);
            var transactionCount = sales.Count;
            var averageTransaction = transactionCount > 0 ? totalSales / transactionCount : 0;

            // รายงานตามช่องทางการชำระเงิน
            var paymentMethods = sales
                .GroupBy(s => s.PaymentMethod)
                .Select(g => new PaymentMethodSummaryDto
                {
                    PaymentMethod = g.Key.ToString(),
                    Amount = g.Sum(s => s.NetAmount),
                    Count = g.Count()
                })
                .ToList();

            // รายงานยอดขายรายชั่วโมง
            var hourlySales = sales
                .GroupBy(s => s.SaleDate.Hour)
                .Select(g => new HourlySalesDto
                {
                    Hour = g.Key,
                    Amount = g.Sum(s => s.NetAmount),
                    TransactionCount = g.Count()
                })
                .OrderBy(h => h.Hour)
                .ToList();

            return new DailySalesReportDto
            {
                Date = date,
                TotalSales = totalSales,
                TransactionCount = transactionCount,
                AverageTransaction = averageTransaction,
                PaymentMethods = paymentMethods,
                HourlySales = hourlySales
            };
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int limit = 10)
        {
            var topProducts = await (from si in _context.SaleItems
                                   join s in _context.Sales on si.SaleId equals s.Id
                                   where s.SaleDate >= startDate && s.SaleDate <= endDate
                                   group si by new { si.ProductId, si.ProductName } into g
                                   select new TopSellingProductDto
                                   {
                                       ProductId = g.Key.ProductId,
                                       ProductName = g.Key.ProductName,
                                       QuantitySold = g.Sum(si => si.Quantity),
                                       TotalRevenue = g.Sum(si => si.TotalAmount),
                                       TransactionCount = g.Count()
                                   })
                                   .OrderByDescending(p => p.QuantitySold)
                                   .Take(limit)
                                   .ToListAsync();

            return topProducts;
        }

        public async Task<MonthlySalesReportDto> GetMonthlySalesReportAsync(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate < endDate)
                .ToListAsync();

            var totalSales = sales.Sum(s => s.NetAmount);
            var transactionCount = sales.Count;
            var averageTransaction = transactionCount > 0 ? totalSales / transactionCount : 0;

            // ยอดขายรายวัน
            var dailySales = sales
                .GroupBy(s => s.SaleDate.Date)
                .Select(g => new DailySalesDto
                {
                    Date = g.Key,
                    Amount = g.Sum(s => s.NetAmount),
                    TransactionCount = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();

            // เปรียบเทียบกับเดือนก่อน
            var lastMonthStart = startDate.AddMonths(-1);
            var lastMonthEnd = lastMonthStart.AddMonths(1);
            var lastMonthSales = await _context.Sales
                .Where(s => s.SaleDate >= lastMonthStart && s.SaleDate < lastMonthEnd)
                .SumAsync(s => s.NetAmount);

            var growthRate = lastMonthSales > 0 ? 
                ((totalSales - lastMonthSales) / lastMonthSales * 100) : 0;

            return new MonthlySalesReportDto
            {
                Year = year,
                Month = month,
                TotalSales = totalSales,
                TransactionCount = transactionCount,
                AverageTransaction = averageTransaction,
                GrowthRate = growthRate,
                DailySales = dailySales
            };
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var today = DateTime.Now.Date;
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var lastMonth = thisMonth.AddMonths(-1);

            // ยอดขายวันนี้
            var todaySales = await _context.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => s.NetAmount);

            // ยอดขายเดือนนี้
            var thisMonthSales = await _context.Sales
                .Where(s => s.SaleDate >= thisMonth)
                .SumAsync(s => s.NetAmount);

            // จำนวนการทำรายการวันนี้
            var todayTransactions = await _context.Sales
                .CountAsync(s => s.SaleDate.Date == today);

            // สินค้าสต็อกต่ำ
            var lowStockCount = await _context.Inventories
                .CountAsync(i => i.Quantity <= i.MinStock);

            // สินค้าใกล้หมดอายุ (7 วัน)
            var expiringCount = await _context.Inventories
                .CountAsync(i => i.ExpiryDate.HasValue && 
                               i.ExpiryDate <= DateTime.Now.AddDays(7));

            // รายการขายล่าสุด
            var recentSales = await _context.Sales
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
                .Select(s => new RecentSaleDto
                {
                    SaleNo = s.SaleNo,
                    Amount = s.NetAmount,
                    SaleDate = s.SaleDate,
                    PaymentMethod = s.PaymentMethod.ToString()
                })
                .ToListAsync();

            // กราฟยอดขาย 7 วันล่าสุด
            var chartData = new List<SalesChartDto>();
            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                var amount = await _context.Sales
                    .Where(s => s.SaleDate.Date == date)
                    .SumAsync(s => s.NetAmount);

                chartData.Add(new SalesChartDto
                {
                    Date = date,
                    Amount = amount,
                    Label = date.ToString("MM/dd")
                });
            }

            return new DashboardSummaryDto
            {
                TodaySales = todaySales,
                ThisMonthSales = thisMonthSales,
                TodayTransactions = todayTransactions,
                LowStockCount = lowStockCount,
                ExpiringProductsCount = expiringCount,
                RecentSales = recentSales,
                SalesChart = chartData
            };
        }

        public async Task<List<SalesChartDto>> GetSalesChartDataAsync(DateTime startDate, DateTime endDate, string period = "daily")
        {
            var sales = await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();

            if (period == "daily")
            {
                return sales
                    .GroupBy(s => s.SaleDate.Date)
                    .Select(g => new SalesChartDto
                    {
                        Date = g.Key,
                        Amount = g.Sum(s => s.NetAmount),
                        Label = g.Key.ToString("MM/dd")
                    })
                    .OrderBy(c => c.Date)
                    .ToList();
            }
            else if (period == "monthly")
            {
                return sales
                    .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                    .Select(g => new SalesChartDto
                    {
                        Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                        Amount = g.Sum(s => s.NetAmount),
                        Label = $"{g.Key.Month:D2}/{g.Key.Year}"
                    })
                    .OrderBy(c => c.Date)
                    .ToList();
            }

            return new List<SalesChartDto>();
        }

        public async Task<ProfitAnalysisDto> GetProfitAnalysisAsync(DateTime startDate, DateTime endDate)
        {
            var saleItems = await (from si in _context.SaleItems
                                 join s in _context.Sales on si.SaleId equals s.Id
                                 where s.SaleDate >= startDate && s.SaleDate <= endDate
                                 select si)
                                 .ToListAsync();

            var totalRevenue = saleItems.Sum(si => si.TotalAmount);
            
            // คำนวณ Cost ของสินค้าที่ขาย
            var totalCost = 0m;
            foreach (var item in saleItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    totalCost += product.Cost * item.Quantity;
                }
            }

            var grossProfit = totalRevenue - totalCost;
            var profitMargin = totalRevenue > 0 ? (grossProfit / totalRevenue * 100) : 0;

            // รายการสินค้าที่กำไรสูงสุด
            var productProfits = new List<ProductProfitDto>();
            var groupedItems = saleItems.GroupBy(si => si.ProductId);

            foreach (var group in groupedItems)
            {
                var product = await _context.Products.FindAsync(group.Key);
                if (product != null)
                {
                    var revenue = group.Sum(si => si.TotalAmount);
                    var cost = product.Cost * group.Sum(si => si.Quantity);
                    var profit = revenue - cost;

                    productProfits.Add(new ProductProfitDto
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Revenue = revenue,
                        Cost = cost,
                        Profit = profit,
                        ProfitMargin = revenue > 0 ? (profit / revenue * 100) : 0
                    });
                }
            }

            return new ProfitAnalysisDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = totalRevenue,
                TotalCost = totalCost,
                GrossProfit = grossProfit,
                ProfitMargin = profitMargin,
                ProductProfits = productProfits.OrderByDescending(p => p.Profit).ToList()
            };
        }
    }
}