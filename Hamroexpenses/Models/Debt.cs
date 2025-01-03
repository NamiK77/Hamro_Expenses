namespace Hamroexpenses.Models
{
    public class Debt
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsCleared { get; set; } = false; // Indicates whether the debt is paid off

        // New properties
        public List<string> Tags { get; set; } = new List<string>();  // Tags list
        public string Note { get; set; } = string.Empty;  // Note for debt
    }
}
