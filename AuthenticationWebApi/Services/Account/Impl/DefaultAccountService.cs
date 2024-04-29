using AutoMapper;
using AuthenticationWebApi.Dtos.Account;
using AuthenticationWebApi.Helpers.ApplicationException;
using AuthenticationWebApi.Helpers.Constant;
using AuthenticationWebApi.Helpers.DbContext;
using AuthenticationWebApi.Helpers.Jwt;
using AuthenticationWebApi.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Services.Account.Impl
{
    public class DefaultAccountService : BaseService, IAccountService
    {
        private readonly AppPostgreSQLDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;

        public DefaultAccountService(
            AppPostgreSQLDbContext context,
            IOptions<AppSettings> appSettings,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            IJwtUtils jwtUtils,
            IHttpContextAccessor httpContextAccessor
            ) : base(httpContextAccessor)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
        }

        public async Task<AccountDto> CreateAsync(RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                throw new BadRequestException("email", $"User with email = {request.Email} already exists");

            var notFoundRoles = new List<string>();
            foreach (var role in request.Roles)
            {
                if (await _roleManager.FindByNameAsync(role) == null)
                    notFoundRoles.Add(role);
            }

            if (notFoundRoles.Count > 0)
                throw new NotFoundException(string.Join('\n', notFoundRoles.Select(role => $"role = {role} NotFound")));

            User account = new User
            {
                Email = request.Email,
                UserName = request.Email,
                Surname = request.Surname,
                Name = request.Name,
                Patronymic = request.Patronymic,
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(account, request.Password);
            if (!result.Succeeded)
                throw new AppException($"failed to create account by email = {request.Email}\n" +
                    result.Errors.Select(error => $"Code = {error.Code}\tDescription = {error.Description}")
                           .Aggregate((current, next) => $"{current}\n{next}"));

            foreach (var role in request.Roles)
            {
                await _userManager.AddToRoleAsync(account, role);
            }

            AccountDto response = _mapper.Map<AccountDto>(account);

            return response;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var account = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (account is null)
                throw new NotFoundException("id", $"Account by id = {id} Not found");

            var result = await _userManager.DeleteAsync(account);
            if (!result.Succeeded)
                throw new AppException($"failed to delete account by id = {id}\n" +
                    result.Errors.Select(error => $"Code = {error.Code}\tDescription = {error.Description}")
                        .Aggregate((current, next) => $"{current}\n{next}"));

            return id;
        }

        public AccountDto Get(string id)
        {
            var account = _userManager.Users
                .FirstOrDefault(u => u.Id == id);

            if (account is null)
                throw new NotFoundException("id", $"Account by id = {id} Not found");

            AccountDto response = _mapper.Map<AccountDto>(account);

            return response;
        }

        public List<AccountDto> GetAll()
        {
            var accounts = _userManager.Users.ToList();

            List<AccountDto> response = _mapper.Map<List<AccountDto>>(accounts);

            return response;
        }

        public async Task<List<string>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => r.Name).ToListAsync();

            return roles;
        }

        public async Task<AuthenticateResponse> LoginAsync(LoginRequest request)
        {
            var account = await _userManager.Users
                .Include(a => a.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // validate
            if (account is null || !await _userManager.CheckPasswordAsync(account, request.Password))
                throw new BadRequestException("Email or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(account);
            var refreshToken = generateRefreshToken();
            account.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(account);

            // save changes to db
            _context.Update(account);
            _context.SaveChanges();

            AuthenticateResponse response =_mapper.Map<AuthenticateResponse>(account);
            response.AccessToken = jwtToken;
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        public async Task<AuthenticateResponse> RefreshTokenAsync(string token)
        {
            var account = await getAccountByRefreshToken(token);
            var refreshToken = account.RefreshTokens.First(x => x.Token == token);

            if (refreshToken.IsExpired)
                throw new BadRequestException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = generateRefreshToken();
            account.RefreshTokens.Add(newRefreshToken);

            _context.Remove(refreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(account);

            // save changes to db
            _context.Update(account);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(account);

            // return data in authenticate response object
            var response = _mapper.Map<AuthenticateResponse>(account);
            response.AccessToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;

            return response;
        }


        // hepler methods
        private RefreshToken generateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                // token is a cryptographically strong random sequence of values
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                // token is valid for
                Expires = DateTime.Now.AddMinutes(_appSettings.RefreshTokenTTL),
                Created = DateTime.Now,
            };

            // ensure token is unique by checking against db
            var tokenIsUnique = !_userManager.Users.Any(a => a.RefreshTokens.Any(t => t.Token == refreshToken.Token));

            if (!tokenIsUnique)
                return generateRefreshToken();

            return refreshToken;
        }

        private async Task<User> getAccountByRefreshToken(string token)
        {
            var account = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == token));

            if (account is null)
                throw new BadRequestException("Invalid token");

            return account;
        }

        private void removeOldRefreshTokens(User account)
        {
            account.RefreshTokens.RemoveAll(x => x.IsExpired);
        }
    }
}
