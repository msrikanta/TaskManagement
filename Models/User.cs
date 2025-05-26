using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class User
    {
        [Key] // Explicitly marks as Primary Key (optional if named "Id")
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; } // Store hashed password

        [Required]
        public string Role { get; set; } = "User"; // "User" or "Admin"

        // Navigation Property: One user can have many tasks
        public required ICollection<TaskItem> Tasks { get; set; }
    }
}

