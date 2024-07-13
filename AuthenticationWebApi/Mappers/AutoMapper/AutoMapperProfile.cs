using AuthenticationWebApi.Dtos.Account;
using AuthenticationWebApi.Models.Account;
using AutoMapper;
using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Mappers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthenticateResponse>();

            CreateMap<User, AccountDto>();

            CreateMap<AccountSettings, AccountSettingsDto>();
            CreateMap<AccountSettingsDto, AccountSettings>();
            CreateMap<CreateAccountSettingsDto, AccountSettings>();
        }
    }
}
