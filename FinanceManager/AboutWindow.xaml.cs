using System.Windows;
namespace FinanceManager
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            NameText.Text = "Omid Torabi Mofrad";
            EmailText.Text = "omidtorabi986@gmail.com";
            SkillText.Text = " | computer engineering | C# | WPF | c++ | python | sof dev | ";
            AboutText.Text = "Hello, I'm Omid, a second-semester Computer Engineering student. This project is my first official software project.\r\n\r\nIf you have any questions, feedback, or suggestions, I'd be happy to hear from you. Please feel free to contact me via email.\r\n\r\nI hope you enjoy using the application!";
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}