using AutoMapper;
using AuthenticationWebApi.Dtos.Account;
using User = AuthenticationWebApi.Models.Account.Account;

namespace AuthenticationWebApi.Mapper.AutoMapper
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
