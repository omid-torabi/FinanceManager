using System;
using System.IO;
using FinanceManager.Helpers;
namespace FinanceManager.Services
{
    public class SecurityService
    {
        private readonly string _settingsPath;
        public SecurityService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appData, "FinanceManager");

            if (!Directory.Exists(appFolder))
                Directory.CreateDirectory(appFolder);
            _settingsPath = Path.Combine(appFolder, "security.json");
        }
        public bool IsFirstRun()
        {
            return !File.Exists(_settingsPath);
        }
        public void SavePassword(string password)
        {
            string hashedPassword = PasswordHelper.HashPassword(password);
            string json = $"{{\"PasswordHash\":\"{hashedPassword}\",\"CreatedDate\":\"{DateTime.Now}\"}}";
            File.WriteAllText(_settingsPath, json);
        }
        public bool ValidatePassword(string password)
        {
            if (!File.Exists(_settingsPath))
                return false;

            string json = File.ReadAllText(_settingsPath);
            string storedHash = "";
            string searchKey = "\"PasswordHash\":\"";
            int startIndex = json.IndexOf(searchKey) + searchKey.Length;
            int endIndex = json.IndexOf("\"", startIndex);
            if (startIndex > 0 && endIndex > 0)
            {
                storedHash = json.Substring(startIndex, endIndex - startIndex);
            }
            string enteredHash = PasswordHelper.HashPassword(password);
            return enteredHash == storedHash;
        }
    }
}