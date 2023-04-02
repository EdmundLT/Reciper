using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reciper.Data;
using Reciper.Model.User;

namespace Reciper.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        User user = new User();
        CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
        user.Username = request.username;
        user.Email = request.email;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        
        //Add to Database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        string jwtoken = CreateToken(user);
        HttpContext.Response.Headers.Add("Authorization", "Bearer " + jwtoken);
        return Ok(user);

    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == request.username);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (!VerifyPassword(request.password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("username or password incorrect.");
        }

        string jwtoken = CreateToken(user);
        HttpContext.Response.Headers.Add("Authorization", "Bearer " + jwtoken);
        return Ok(jwtoken);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred);
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA256())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA256(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
    
    
}