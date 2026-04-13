namespace SmartRecipe.Api.Interfaces
{
    public interface IRecipeAiService
    {
        Task<string> GetAiRecipeAsync(string ingredients);
    }
}