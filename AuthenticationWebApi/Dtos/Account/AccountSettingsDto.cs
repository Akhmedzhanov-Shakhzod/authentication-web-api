using AuthenticationWebApi.Helpers.Enums;
using System.Text.Json.Serialization;

namespace AuthenticationWebApi.Dtos.Account
{
    public class AccountSettingsDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Language Language { get; set; }
        public string Link { get; set; }
    }
}
