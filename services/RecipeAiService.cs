using System.Text;
using System.Text.Json;
using SmartRecipe.Api.Interfaces;

namespace SmartRecipe.Api.Services
{
    public class RecipeAiService : IRecipeAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RecipeAiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["GeminiApiKey"]!;
        }

        public async Task<string> GetAiRecipeAsync(string ingredients)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = $"Generate a healthy recipe using: {ingredients}. Return as JSON." } } }
                }
            };

            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}