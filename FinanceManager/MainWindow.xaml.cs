using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FinanceManager.Models;
using FinanceManager.Services;

namespace FinanceManager
{
    public partial class MainWindow : Window
    {
        private readonly TransactionService _service;
        private List<Transaction> _allTransactions = new List<Transaction>();

        public MainWindow()
        {
            InitializeComponent();
            _service = new TransactionService();
            LoadTransactions();
            UpdateBalance();
        }

        private void LoadTransactions()
        {
            _allTransactions = _service.GetAllTransactions();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _allTransactions.AsEnumerable();

            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(t => t.Title.ToLower().Contains(searchText) ||
                                               t.Category.ToLower().Contains(searchText));
            }

            if (TypeFilterComboBox.SelectedIndex == 1) // Income
            {
                filtered = filtered.Where(t => t.Type == TransactionType.Income);
            }
            else if (TypeFilterComboBox.SelectedIndex == 2) // Expense
            {
                filtered = filtered.Where(t => t.Type == TransactionType.Expense);
            }

            TransactionsGrid.ItemsSource = filtered.ToList();
        }

        private void UpdateBalance()
        {
            var balance = _service.GetBalance();
            BalanceText.Text = $"Balance: {balance:N0} €";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            TypeFilterComboBox.SelectedIndex = 0;
            ApplyFilters();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddTransactionWindow();
            addWindow.Owner = this;
            if (addWindow.ShowDialog() == true)
            {
                LoadTransactions();
                UpdateBalance();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsGrid.SelectedItem is Transaction selected)
            {
                var editWindow = new EditTransactionWindow(selected);
                editWindow.Owner = this;
                if (editWindow.ShowDialog() == true)
                {
                    LoadTransactions();
                    UpdateBalance();
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionsGrid.SelectedItem is Transaction selected)
            {
                var result = MessageBox.Show($"Delete transaction \"{selected.Title}\"?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _service.DeleteTransaction(selected.Id);
                    LoadTransactions();
                    UpdateBalance();
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var reportWindow = new ReportWindow();
            reportWindow.Owner = this;
            reportWindow.ShowDialog();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    DefaultExt = ".json",
                    FileName = $"Backup_{DateTime.Now:yyyy-MM-dd}"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string json = _service.ExportToJson();
                    System.IO.File.WriteAllText(saveFileDialog.FileName, json);
                    MessageBox.Show("Backup saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving backup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    DefaultExt = ".json"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var result = MessageBox.Show("All current transactions will be replaced. Continue?",
                        "Confirm Restore", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        string json = System.IO.File.ReadAllText(openFileDialog.FileName);
                        _service.ImportFromJson(json);
                        LoadTransactions();
                        UpdateBalance();
                        MessageBox.Show("Restore completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Owner = this;
            aboutWindow.ShowDialog();
        }
    }
}