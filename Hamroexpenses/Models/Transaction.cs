namespace Hamroexpenses.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // Inflow, Outflow, or Debt
        public string? Tags { get; set; } // Optional tags/labels
        public string? Notes { get; set; } // Optional notes
        public bool IsCleared { get; set; } // For debt transactions
    }
}
