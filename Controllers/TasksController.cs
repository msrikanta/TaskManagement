using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data; 
using TaskManagement.DTOs;
using TaskManagement.Models; 
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context; // ✅ Add DbContext

        public TasksController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // ✅ Register user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result == "User already exists")
                return BadRequest(result);

            return Ok(result);
        }

        // ✅ Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }

        // ✅ Update task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
                return NotFound("Task not found");

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.Status = updatedTask.Status;

            await _context.SaveChangesAsync();
            return Ok("Task updated successfully");
        }

        // ✅ Delete task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok("Task deleted successfully");
        }
    }
}
