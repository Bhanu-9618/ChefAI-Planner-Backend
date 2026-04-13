using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPdfService _pdfService;

        public RecipeController(IRecipeAiService aiService, AppDbContext context, IPdfService pdfService)
        {
            _aiService = aiService;
            _context = context;
            _pdfService = pdfService;
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return Unauthorized(new { message = $"User not found. Claim Name: {username}" });

            recipe.UserId = user.Id;
            recipe.CreatedAt = DateTime.UtcNow;

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Recipe saved successfully!", recipeId = recipe.Id });
        }

        [HttpGet("my-recipes")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetMyRecipes()
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return Unauthorized();

            var recipes = await _context.Recipes
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null) return NotFound("Recipe not found");

            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || recipe.UserId != user.Id)
            {
                return Unauthorized("You don't have permission to view this recipe");
            }

            return Ok(recipe);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipes([FromQuery] string title)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return Unauthorized();

            var recipes = await _context.Recipes
                .Where(r => r.UserId == user.Id && r.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();

            return Ok(recipes);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadRecipePdf(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null) return NotFound("Recipe not found");

            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || recipe.UserId != user.Id)
            {
                return Unauthorized("You don't have permission to download this recipe");
            }

            var pdfBytes = _pdfService.GenerateRecipePdf(recipe);

            return File(pdfBytes, "application/pdf", $"{recipe.Title.Replace(" ", "_")}.pdf");
        }
    }
}