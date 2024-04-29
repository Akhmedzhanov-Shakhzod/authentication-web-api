using AutoMapper;
using AuthenticationWebApi.Dtos.Account;
using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Mappers.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthenticateResponse>();

            CreateMap<User, AccountDto>();
        }
    }
}
