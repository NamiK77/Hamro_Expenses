using Hamroexpenses.Models;
using System.Text.Json;
using System.Linq;

namespace Hamroexpenses.Services
{
    public class IncomeService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "incomes.json");

        private List<Income> _incomes;

        public IncomeService()
        {
            EnsureIncomeFileExists();
            _incomes = LoadIncomes();
        }

        private void EnsureIncomeFileExists()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(FolderPath);
                File.WriteAllText(FilePath, "[]");
            }
        }

        private List<Income> LoadIncomes()
        {
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Income>>(json) ?? new List<Income>();
        }

        private void SaveIncomes()
        {
            var json = JsonSerializer.Serialize(_incomes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public List<Income> GetAllIncomes()
        {
            return _incomes;
        }

        public void AddIncome(Income income)
        {
            income.Id = _incomes.Count > 0 ? _incomes.Max(i => i.Id) + 1 : 1;
            _incomes.Add(income);
            SaveIncomes();
        }

        public void UpdateIncome(Income income)
        {
            var existing = _incomes.FirstOrDefault(i => i.Id == income.Id);
            if (existing != null)
            {
                existing.Amount = income.Amount;
                existing.Date = income.Date;
                existing.Description = income.Description;
                existing.Category = income.Category;
                SaveIncomes();
            }
        }

        public void DeleteIncome(int id)
        {
            var income = _incomes.FirstOrDefault(i => i.Id == id);
            if (income != null)
            {
                _incomes.Remove(income);
                SaveIncomes();
            }
        }

        public List<Income> GetHighestIncome(int count = 5)
        {
            return _incomes.OrderByDescending(i => i.Amount).Take(count).ToList();
        }

        public List<Income> GetLowestIncome(int count = 5)
        {
            return _incomes.OrderBy(i => i.Amount).Take(count).ToList();
        }

        // Deduct income after clearing a debt
        public void DeductIncome(decimal amount)
        {
            // Ensure the amount is greater than zero before deducting
            if (amount <= 0) return;

            // Get the total income
            var totalIncome = _incomes.Sum(i => i.Amount);

            // Ensure the amount does not exceed the total income
            if (amount > totalIncome) amount = totalIncome;

            // Proportionally deduct the amount from each income entry
            foreach (var income in _incomes)
            {
                var proportion = income.Amount / totalIncome;
                income.Amount -= amount * proportion;
            }

            SaveIncomes(); // Save the updated incomes
        }
    }
}
