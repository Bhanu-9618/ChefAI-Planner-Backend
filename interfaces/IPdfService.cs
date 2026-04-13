using SmartRecipe.Api.Models;

namespace SmartRecipe.Api.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateRecipePdf(Recipe recipe);
    }
}