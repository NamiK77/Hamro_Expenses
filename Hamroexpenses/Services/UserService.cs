﻿using Hamroexpenses.Models;
using System.Text.Json;

namespace Hamroexpenses.Services
{
    public class UserService
    {
        private static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string FolderPath = Path.Combine(DesktopPath, "LocalDB");
        private static readonly string FilePath = Path.Combine(FolderPath, "users.json");

        public User? CurrentUser { get; private set; } // Property for current logged-in user

        public List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
            {
                Directory.CreateDirectory(FolderPath);
                File.WriteAllText(FilePath, "[]");
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(FilePath, json);
        }

        public string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public bool ValidatePassword(string inputPassword, string storedHashedPassword)
        {
            return HashPassword(inputPassword) == storedHashedPassword;
        }

        public bool Login(string username, string password)
        {
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && ValidatePassword(password, user.Password))
            {
                CurrentUser = user; // Set the current logged-in user
                return true;
            }

            return false;
        }

        public void Logout()
        {
            CurrentUser = null; // Clear the current user to log out
        }
    }
}