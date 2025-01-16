
namespace Hamroexpenses.Models
{
    public class User
    {
        public int Id { get; set; } // Unique Id for each user
        public string Username { get; set; } // Username for login
        public string Password { get; set; } // Hashed password for authentication
        public string Email { get; set; } // User's email address
        public string Currency { get; set; } // User's preferred currency
    }
}
