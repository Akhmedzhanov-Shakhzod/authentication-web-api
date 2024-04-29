namespace AuthenticationWebApi.Dtos.Account
{
    public class AccountDto
    {
        public string Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
