using Microsoft.AspNetCore.Identity;

namespace MarketPlaceWebApi.Models.Account
{
    public class Account : IdentityUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
