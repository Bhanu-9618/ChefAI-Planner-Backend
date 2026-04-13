using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecipe.Api.Data;
using SmartRecipe.Api.Interfaces;
using SmartRecipe.Api.Models;
using System.Text.Json;

namespace SmartRecipe.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeAiService _aiService;
        private readonly AppDbContext _context;

        public RecipeController(IRecipeAiService aiService, AppDbContext context)
        {
            _aiService = aiService;
            _context = context;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<Recipe>> GenerateRecipe([FromBody] string ingredients)
        {
            if (string.IsNullOrEmpty(ingredients)) return BadRequest("Ingredients cannot be empty");

            try
            {
                var aiResponse = await _aiService.GetAiRecipeAsync(ingredients);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var recipe = JsonSerializer.Deserialize<Recipe>(aiResponse, options);

                if (recipe == null || string.IsNullOrEmpty(recipe.Title))
                {
                    return BadRequest(new { message = "Invalid AI response format", raw = aiResponse });
                }

                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Recipe generation failed", details = ex.Message });
            }
        }

        [HttpPost("save")]
        public async Task<ActionResult> SaveRecipe([FromBody] Recipe recipe)
        {
            if (recipe == null) return BadRequest("Recipe data is required");

            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null) return Unauthorized(new { message = $"User not found. Claim Name: {username}" });

            recipe.UserId = user.Id;
            recipe.CreatedAt = DateTime.UtcNow;

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recipe saved successfully!", recipeId = recipe.Id });
        }
    }
}