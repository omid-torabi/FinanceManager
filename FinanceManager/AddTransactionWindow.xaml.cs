using System;
using System.Windows;
using System.Windows.Controls;
using FinanceManager.Models;
using FinanceManager.Services;

namespace FinanceManager
{
    public partial class AddTransactionWindow : Window
    {
        private readonly TransactionService _service;

        public AddTransactionWindow()
        {
            InitializeComponent();
            _service = new TransactionService();
            DatePicker.SelectedDate = DateTime.Now;
            LoadCategories();
        }

        private void LoadCategories()
        {
            CategoryComboBox.Items.Add("Salary");
            CategoryComboBox.Items.Add("Food");
            CategoryComboBox.Items.Add("Rent");
            CategoryComboBox.Items.Add("Bills");
            CategoryComboBox.Items.Add("Entertainment");
            CategoryComboBox.Items.Add("Shopping");
            CategoryComboBox.Items.Add("Health");
            CategoryComboBox.Items.Add("Education");
            CategoryComboBox.Items.Add("Other");
            CategoryComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    MessageBox.Show("Please enter a title.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TitleTextBox.Focus();
                    return;
                }

                if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    AmountTextBox.Focus();
                    return;
                }

                if (DatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Please select a date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string typeTag = (TypeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "Expense";
                TransactionType transactionType = typeTag == "Income" ? TransactionType.Income : TransactionType.Expense;

                var transaction = new Transaction
                {
                    Title = TitleTextBox.Text.Trim(),
                    Amount = amount,
                    Date = DatePicker.SelectedDate.Value,
                    Category = CategoryComboBox.Text,
                    Type = transactionType
                };

                _service.AddTransaction(transaction);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}