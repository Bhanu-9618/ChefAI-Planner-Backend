namespace SmartRecipe.Api.DTOs
{
    public class UserUpdateDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string? Goal { get; set; }
    }
}
