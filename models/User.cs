using System.ComponentModel.DataAnnotations;

namespace SmartRecipe.Api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public double Weight { get; set; }
        public double Height { get; set; }
        public string Goal { get; set; } = string.Empty;
    }
}