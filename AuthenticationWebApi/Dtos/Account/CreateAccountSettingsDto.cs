using AuthenticationWebApi.Helpers.ApplicationException;
using AuthenticationWebApi.Helpers.Enums;
using System.Text.Json.Serialization;

namespace AuthenticationWebApi.Dtos.Account
{
    public class CreateAccountSettingsDto
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public Language Lang
        {
            get;
            private set;
        }
        public string Language
        {
            get => Lang.ToString();
            set
            {
                try
                {
                    Lang = (Language)Enum.Parse(typeof(Language), value);
                }
                catch (ArgumentException)
                {
                    throw new AppException($"invalid Language value: select from: [{string.Join(", ", (Language[])Enum.GetValues(typeof(Language)))}]");
                }
            }
        }
        public string Link { get; set; }
    }
}
