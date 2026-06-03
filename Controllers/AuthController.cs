using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BooksAndQuotesApplication.Dtos;
using BooksAndQuotesApplication.Data;
using BooksAndQuotesApplication.Models;


//namespace BooksAndQuotesApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // ================= REGISTER =================
    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid data");

        var username = dto.Username?.Trim();
        var password = dto.Password?.Trim();

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return BadRequest("Username and password are required");
        }

        if (_context.Users.Any(x => x.Username == username))
            return BadRequest("User already exists");

        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        try
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "User registered successfully"
            });
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // ================= LOGIN =================
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto dto)
    {
        if (dto == null)
            return BadRequest("Invalid data");

        var username = dto.Username?.Trim();
        var password = dto.Password?.Trim();

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return BadRequest("Username and password are required");
        }

        var user = _context.Users.FirstOrDefault(x => x.Username == username);

        if (user == null)
            return Unauthorized("Invalid username or password");

        bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!verified)
            return Unauthorized("Invalid username or password");

        var token = GenerateToken(user);

        return Ok(new { token });
    }

    // ================= JWT TOKEN =================
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}