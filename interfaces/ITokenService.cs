using SmartRecipe.Api.Models;

namespace SmartRecipe.Api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}