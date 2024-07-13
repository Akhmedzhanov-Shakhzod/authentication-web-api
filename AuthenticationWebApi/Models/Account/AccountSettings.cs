using AuthenticationWebApi.Helpers.Enums;
using System.Text.Json.Serialization;

namespace AuthenticationWebApi.Models.Account
{
    public class AccountSettings
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Language Language { get; set; }
        public string Link { get; set; }
    }
}
