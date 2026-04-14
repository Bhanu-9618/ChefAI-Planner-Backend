namespace SmartRecipe.Api.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Goal { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}