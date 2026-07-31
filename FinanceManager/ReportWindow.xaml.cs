using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FinanceManager.Models;
using FinanceManager.Services;

namespace FinanceManager
{
    public partial class ReportWindow : Window
    {
        private readonly ReportService _reportService;

        public ReportWindow()
        {
            InitializeComponent();
            _reportService = new ReportService();

            LoadYearsAndMonths();
            LoadReport();
        }

        private void LoadYearsAndMonths()
        {
            var currentYear = DateTime.Now.Year;
            for (int i = currentYear - 5; i <= currentYear + 1; i++)
            {
                YearComboBox.Items.Add(i);
            }
            YearComboBox.SelectedItem = currentYear;

            for (int i = 1; i <= 12; i++)
            {
                MonthComboBox.Items.Add(new DateTime(1, i, 1).ToString("MMMM"));
            }
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void LoadReport()
        {
            if (YearComboBox.SelectedItem == null || MonthComboBox.SelectedItem == null)
                return;

            var year = (int)YearComboBox.SelectedItem;
            var month = MonthComboBox.SelectedIndex + 1;

            var report = _reportService.GetMonthlyReport(year, month);

            IncomeText.Text = $"{report.TotalIncome:N0} Euro";
            ExpenseText.Text = $"{report.TotalExpense:N0} Euro";
            BalanceText.Text = $"{report.Balance:N0} Euro";

            BalanceText.Foreground = report.Balance >= 0
                ? System.Windows.Media.Brushes.Green
                : System.Windows.Media.Brushes.Red;

            var expenseCategories = _reportService.GetCategorySummary(year, month, TransactionType.Expense);
            var incomeCategories = _reportService.GetCategorySummary(year, month, TransactionType.Income);

            var allCategories = new Dictionary<string, decimal>();

            foreach (var cat in incomeCategories)
                allCategories[$"Income → {cat.Key}"] = cat.Value;

            foreach (var cat in expenseCategories)
                allCategories[$"Expense → {cat.Key}"] = cat.Value;

            CategoryGrid.ItemsSource = allCategories
                .OrderByDescending(x => x.Value)
                .ToList();
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadReport();
        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadReport();
        private void RefreshReportButton_Click(object sender, RoutedEventArgs e) => LoadReport();
    }
}