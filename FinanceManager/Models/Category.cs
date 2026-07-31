using System.Collections.Generic;
namespace FinanceManager .Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string icon { get; set; } = "📁";
        public string Type { get; set; } = string.Empty;
        public List<Transaction> Transactions { get; set; } = new();
    }
}