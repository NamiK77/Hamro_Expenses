using Hamroexpenses.Models;
using System.Text.Json;

namespace Hamroexpenses.Services
{
    public class TransactionService
    {
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "LocalDB", "transactions.json");

        public List<Transaction> LoadTransactions()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
                File.WriteAllText(FilePath, "[]");
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        public void SaveTransactions(List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public void AddTransaction(Transaction transaction)
        {
            var transactions = LoadTransactions();
            transactions.Add(transaction);
            SaveTransactions(transactions);
        }

        public List<Transaction> GetAllTransactions()
        {
            return LoadTransactions();
        }
    }
}
