using System;
namespace FinanceManager.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public TransactionType Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string AmountFormatted => Amount.ToString("N0") + " €";
        public string DateFormatted => Date.ToString("yyyy/MM/dd");
    }
}