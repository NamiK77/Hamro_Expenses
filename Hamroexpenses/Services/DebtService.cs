using Hamroexpenses.Models;
using System.Text.Json;

namespace Hamroexpenses.Services
{
    public class DebtService
    {
        private readonly string FilePath;
        private readonly IncomeService _incomeService; // Inject IncomeService to update total income

        public DebtService(IncomeService incomeService)
        {
            _incomeService = incomeService; // Inject IncomeService
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var folderPath = Path.Combine(desktopPath, "LocalDB");
            FilePath = Path.Combine(folderPath, "debts.json");

            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(folderPath);
                File.WriteAllText(FilePath, "[]");
            }
        }

        // Load all debts
        public List<Debt> GetAllDebts()
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Debt>>(json) ?? new List<Debt>();
        }

        // Add a new debt
        public void AddDebt(Debt debt)
        {
            var debts = GetAllDebts();
            debt.Id = debts.Any() ? debts.Max(d => d.Id) + 1 : 1;
            debts.Add(debt);
            SaveDebts(debts);
        }

        // Update an existing debt
        public void UpdateDebt(Debt updatedDebt)
        {
            var debts = GetAllDebts();
            var debt = debts.FirstOrDefault(d => d.Id == updatedDebt.Id);
            if (debt != null)
            {
                debt.Description = updatedDebt.Description;
                debt.Amount = updatedDebt.Amount;
                debt.Category = updatedDebt.Category;
                debt.Date = updatedDebt.Date;
                debt.IsCleared = updatedDebt.IsCleared;
                SaveDebts(debts);
            }
        }

        // Delete a debt
        public void DeleteDebt(int id)
        {
            var debts = GetAllDebts();
            var debt = debts.FirstOrDefault(d => d.Id == id);
            if (debt != null)
            {
                debts.Remove(debt);
                SaveDebts(debts);
            }
        }

        // Clear a debt and update total income
        public bool ClearDebt(int debtId)
        {
            var debts = GetAllDebts();
            var debt = debts.FirstOrDefault(d => d.Id == debtId);

            if (debt == null || debt.IsCleared)
            {
                // If debt doesn't exist or is already cleared
                return false;
            }

            // Mark debt as cleared
            debt.IsCleared = true;

            // Deduct the debt amount from total income
            _incomeService.DeductIncome(debt.Amount);

            // Save the updated debt list
            SaveDebts(debts);

            return true;
        }

        // Save debts to file
        private void SaveDebts(List<Debt> debts)
        {
            var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        // Get highest debts
        public List<Debt> GetHighestDebt(int count = 5)
        {
            return GetAllDebts().OrderByDescending(d => d.Amount).Take(count).ToList();
        }

        // Get lowest debts
        public List<Debt> GetLowestDebt(int count = 5)
        {
            return GetAllDebts().OrderBy(d => d.Amount).Take(count).ToList();
        }
    }
}
