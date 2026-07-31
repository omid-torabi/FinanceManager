using System;
using System.Collections.Generic;
using System.Linq;
using FinanceManager.Models;
using FinanceManager.Data;
namespace FinanceManager.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        public ReportService()
        {
            _context = new AppDbContext();
        }
        public MonthlyReport GetMonthlyReport(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            var transactions = _context.Transactions
                .Where(t => t.Date >= start && t.Date <= end)
                .ToList();

            return new MonthlyReport
            {
                Year = year,
                Month = month,
                TotalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                TotalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
            };
        }
        public Dictionary<string, decimal> GetCategorySummary(int year, int month, TransactionType type)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            var transactions = _context.Transactions
                .Where(t => t.Date >= start && t.Date <= end && t.Type == type)
                .ToList();

            return transactions
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }
    }
}