using Microsoft.EntityFrameworkCore;
using FinanceManager.Models;
namespace FinanceManager .Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=finance.db");
        }
    }
}