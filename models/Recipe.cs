using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecipe.Api.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Ingredients { get; set; } = string.Empty;

        [Required]
        public string Instructions { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}