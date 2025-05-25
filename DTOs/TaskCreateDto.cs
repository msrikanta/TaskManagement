using System;

namespace TaskManagement.DTOs
{
    public class TaskCreateDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
