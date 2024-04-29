namespace MarketPlaceWebApi.Dtos.Account
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
