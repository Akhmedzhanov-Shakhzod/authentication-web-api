using MarketPlaceWebApi.Models.Account;

namespace MarketPlaceWebApi.Helpers.Jwt
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(Account account);

        string? ValidateJwtToken(string token);
    }
}
