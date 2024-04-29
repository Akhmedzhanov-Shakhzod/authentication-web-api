using MarketPlaceWebApi.Dtos.Account;
using MarketPlaceWebApi.Helpers.Attribute;
using MarketPlaceWebApi.Helpers.Constant;
using MarketPlaceWebApi.Services.Account;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MarketPlaceWebApi.Controllers.Account
{
    [Authorize(AppConstants.Role_AdminRole)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController(IAccountService service)
        : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([Required] LoginRequest request)
        {
            AuthenticateResponse response = await service.LoginAsync(request);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([Required] string refreshToken)
        {
            AuthenticateResponse response = await service.RefreshTokenAsync(refreshToken);

            return Ok(response);
        }

        [HttpGet("getAccount")]
        public IActionResult Get([Required] string id)
        {
            AccountDto account = service.Get(id);

            return Ok(account);
        }

        [HttpGet("getAccounts")]
        public IActionResult GetAll()
        {
            List<AccountDto> accounts = service.GetAll();

            return Ok(accounts);
        }

        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            List<string> roles = await service.GetRolesAsync();

            return Ok(roles);
        }

        [HttpPost("createAccount")]
        public async Task<IActionResult> Create([Required] RegisterRequest request)
        {
            AccountDto response = await service.CreateAsync(request);

            return Ok(response);
        }

        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> Delete([Required] string id)
        {
            string response = await service.DeleteAsync(id);

            return Ok(response);
        }
    }
}
