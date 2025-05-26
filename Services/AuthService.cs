using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Data;
using TaskManagement.DTOs;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;




public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(UserRegisterDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            return "User already exists";
        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User",
            Tasks = new List<TaskItem>()
        };


        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return "User registered successfully";
    }

    public async Task<string> LoginAsync(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        // JWT Token generation
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:DurationInMinutes"])),
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
