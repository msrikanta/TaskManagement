using TaskManagement.Models;
using TaskManagement.DTOs;

public interface IAuthService
{
    Task<string> RegisterAsync(UserRegisterDto dto);
    Task<string> LoginAsync(UserLoginDto dto);
}

