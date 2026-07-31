using FinanceManager.Models;
using FinanceManager.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FinanceManager
{
    public partial class EditTransactionWindow : Window
    {
        private readonly TransactionService _service;
        private readonly Transaction _transaction;

        public EditTransactionWindow(Transaction transaction)
        {
            InitializeComponent();
            _service = new TransactionService();
            _transaction = transaction;

            LoadCategories();
            LoadTransactionData();
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
        }

        private void LoadTransactionData()
        {
            TitleTextBox.Text = _transaction.Title;
            AmountTextBox.Text = _transaction.Amount.ToString("N0");
            DatePicker.SelectedDate = _transaction.Date;
            CategoryComboBox.Text = _transaction.Category;

            if (_transaction.Type == TransactionType.Income)
                TypeComboBox.SelectedIndex = 0;
            else
                TypeComboBox.SelectedIndex = 1;
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

                _transaction.Title = TitleTextBox.Text.Trim();
                _transaction.Amount = amount;
                _transaction.Date = DatePicker.SelectedDate.Value;
                _transaction.Category = CategoryComboBox.Text;
                _transaction.Type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() == "Income"
                    ? TransactionType.Income
                    : TransactionType.Expense;

                _service.UpdateTransaction(_transaction);
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