﻿using Hamroexpenses.Models;
using System.Text.Json;

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

        // Deduct income after clearing a debt
        public void DeductIncome(decimal amount)
        {
            // Ensure the amount is greater than zero before deducting
            if (amount <= 0) return;

            // Deduct the amount from the total income
            var totalIncome = _incomes.Sum(i => i.Amount);
            totalIncome -= amount;

            // Now, update the income list (we'll reduce the total income proportionally)
            // For simplicity, here we're just adjusting all incomes evenly.
            // You may need to implement more sophisticated logic based on your business logic.
            foreach (var income in _incomes)
            {
                // Adjust the income amounts proportionally (optional)
                // Adjust each income entry by reducing the same percentage across all records.
                income.Amount -= amount / _incomes.Count;
            }

            SaveIncomes(); // Save the updated incomes
        }
    }
}