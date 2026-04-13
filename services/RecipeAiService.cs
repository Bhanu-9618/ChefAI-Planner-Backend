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
            var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={_apiKey}";

            var prompt = $"Act as a professional nutritionist. Generate a healthy recipe using these ingredients: {ingredients}. " +
                         "You MUST return the response ONLY as a JSON object with this structure: " +
                         "{ \"title\": \"string\", \"description\": \"string\", \"ingredients\": \"string\", \"instructions\": \"string\" }. " +
                         "Do not include markdown formatting like ```json or any other text.";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);

            if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
            {
                var recipeJson = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return recipeJson ?? string.Empty;
            }

            return result;
        }
    }
}