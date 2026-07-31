using System.Windows;
using System.Windows.Input;
using FinanceManager.Services;

namespace FinanceManager
{
    public partial class LoginWindow : Window
    {
        private readonly SecurityService _securityService;
        private int _attempts = 0;

        public LoginWindow()
        {
            InitializeComponent();
            _securityService = new SecurityService();

            if (_securityService.IsFirstRun())
            {
                Title = "Setup Password";
                ErrorText.Text = "⚠️ Please set a new password.";
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CheckPassword();
        }

        private void CheckPassword()
        {
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorText.Text = "Please enter your password.";
                return;
            }

            if (_securityService.IsFirstRun())
            {
                if (password.Length < 4)
                {
                    ErrorText.Text = "Password must be at least 4 characters.";
                    return;
                }

                _securityService.SavePassword(password);
                MessageBox.Show("✅ Password set successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // باز کردن MainWindow با ShowDialog
                var mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                Close();
                return;
            }

            if (_securityService.ValidatePassword(password))
            {
                var mainWindow = new MainWindow();
                mainWindow.ShowDialog();
                Close();
            }
            else
            {
                _attempts++;
                ErrorText.Text = $"Wrong password. ({_attempts}/3)";
                PasswordBox.Password = "";

                if (_attempts >= 3)
                {
                    MessageBox.Show("Maximum attempts exceeded.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Application.Current.MainWindow == null)
            {
                Application.Current.Shutdown();
            }
        }
    }
}