using AuthenticationWebApi.Models.Account;

namespace AuthenticationWebApi.Helpers.Jwt
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(Account account);

        (string?, List<string>?) ValidateJwtToken(string token);
    }
}
