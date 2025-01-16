namespace Hamroexpenses.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // New Category property

        public DateTime Date { get; set; } // Ensure this property exists

        // New properties
        public List<string> Tags { get; set; } = new List<string>();  // Tags list
        public string Note { get; set; } = string.Empty;  // Note for expense
    }
}
