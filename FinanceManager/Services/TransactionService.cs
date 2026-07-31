using System;
using System.Collections.Generic;
using System.Linq;
using FinanceManager.Models;
using FinanceManager.Data;
namespace FinanceManager.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;
        public TransactionService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }
        public List<Transaction> GetAllTransactions()
        {
            return _context.Transactions.ToList();
        }
        public string ExportToJson()
        {
            var transactions = _context.Transactions.ToList();
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };
            return System.Text.Json.JsonSerializer.Serialize(transactions, options);
        }
        public void ImportFromJson(string json)
        {
            var transactions = System.Text.Json.JsonSerializer.Deserialize<List<Transaction>>(json);
            if (transactions == null || transactions.Count == 0)
                return;
            _context.Transactions.RemoveRange(_context.Transactions);
            _context.SaveChanges();
            foreach (var transaction in transactions)
            {
                transaction.Id = 0;
                _context.Transactions.Add(transaction);
            }
            _context.SaveChanges();
        }
        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
        public void UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            _context.SaveChanges();
        }
        public void DeleteTransaction(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                _context.SaveChanges();
            }
        }
        public decimal GetBalance()
        {
            var transactions = _context.Transactions.ToList();
            
            decimal totalIncome = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            decimal totalExpense = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            return totalIncome - totalExpense;
        }
    }
}