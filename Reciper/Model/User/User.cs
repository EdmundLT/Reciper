using System.ComponentModel.DataAnnotations;

namespace Reciper.Model.User;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Email { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
}

