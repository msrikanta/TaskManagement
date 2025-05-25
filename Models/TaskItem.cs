using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; } = "Pending"; // e.g., Pending, Completed

        // Foreign key and navigation property
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
