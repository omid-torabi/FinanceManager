# 💰 Personal Finance Manager

A desktop application for managing personal finances, built with **C#** and **WPF** on .NET 8.

---

## 🚀 Features

- ✅ Add, Edit, Delete Transactions
- ✅ Income / Expense Tracking
- ✅ Real-time Balance Display
- ✅ Monthly Reports
- ✅ Search & Filter
- ✅ Password Protection
- ✅ Backup & Restore (JSON)
- ✅ ClickOnce Installer

---

## 🛠️ Technologies Used

- **C#** / **.NET 8**
- **WPF** (Windows Presentation Foundation)
- **Entity Framework Core** (SQLite)
- **OxyPlot** (Charts)
- **MVVM Architecture**

---

## 📦 Installation

1. Download the latest `setup.exe` from [Releases](https://github.com/omid-torabi/FinanceManager/releases)
2. Run the installer
3. Launch from Start Menu or Desktop

---

## 🧪 How to Run from Source

```bash
git clone https://github.com/omid-torabi/FinanceManager.git
cd FinanceManager
dotnet restore
dotnet run
FinanceManager/
├── Models/          # Data models (Transaction, Category, etc.)
├── Services/        # Business logic (TransactionService, ReportService, SecurityService)
├── Helpers/         # Utility classes (PasswordHelper)
├── Data/            # Database context (AppDbContext)
├── Views/           # XAML windows
└── ViewModels/      # MVVM logic (optional)
