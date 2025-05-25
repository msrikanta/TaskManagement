using TaskManagement.DTOs;

namespace TaskManagement.Services
{
    public interface TaskService
    {
        Task<IEnumerable<TaskCreateDto>> GetTasksForUser(Guid userId);
        Task<TaskCreateDto> GetTask(Guid id);
        Task CreateTask(Guid userId, TaskCreateDto dto);
        Task UpdateTask(Guid id, TaskCreateDto dto);
        Task DeleteTask(Guid id);
    }
}

