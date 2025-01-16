using Hamroexpenses.Models;
using System.Text.Json;

namespace Hamroexpenses.Services
{
    public class ExpenseService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "expenses.json");

        private List<Expense> _expenses;

        public ExpenseService()
        {
            EnsureExpenseFileExists();
            _expenses = LoadExpenses();
        }

        private void EnsureExpenseFileExists()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(FolderPath);
                File.WriteAllText(FilePath, "[]");
            }
        }

        private List<Expense> LoadExpenses()
        {
            try
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expenses: {ex.Message}");
                return new List<Expense>(); // Return empty list if there's an error
            }
        }

        private void SaveExpenses()
        {
            try
            {
                var json = JsonSerializer.Serialize(_expenses, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expenses: {ex.Message}");
            }
        }

        public List<Expense> GetAllExpenses()
        {
            return _expenses;
        }

        public void AddExpense(Expense expense)
        {
            try
            {
                expense.Id = _expenses.Count > 0 ? _expenses.Max(e => e.Id) + 1 : 1;
                _expenses.Add(expense);
                SaveExpenses();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding expense: {ex.Message}");
            }
        }

        public void DeleteExpense(int id)
        {
            try
            {
                var expense = _expenses.FirstOrDefault(e => e.Id == id);
                if (expense != null)
                {
                    _expenses.Remove(expense);
                    SaveExpenses();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting expense: {ex.Message}");
            }
        }

        // Method to get the highest expenses
        public List<Expense> GetHighestExpense(int count = 5)
        {
            return _expenses.OrderByDescending(e => e.Amount).Take(count).ToList();
        }

        // Method to get the lowest expenses
        public List<Expense> GetLowestExpense(int count = 5)
        {
            return _expenses.OrderBy(e => e.Amount).Take(count).ToList();
        }
    }
}
