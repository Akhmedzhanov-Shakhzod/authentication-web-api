using AuthenticationWebApi.Dtos.Account;

namespace AuthenticationWebApi.Services.Account
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> LoginAsync(LoginRequest request);

        Task<AuthenticateResponse> RefreshTokenAsync(string token);

        AccountDto Get(string id);

        List<AccountDto> GetAll();

        Task<List<string>> GetRolesAsync();

        Task<AccountDto> CreateAsync(RegisterRequest request);

        Task<string> DeleteAsync(string id);
    }
}
