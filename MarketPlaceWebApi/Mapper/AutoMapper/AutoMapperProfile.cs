using AutoMapper;
using MarketPlaceWebApi.Dtos.Account;
using User = MarketPlaceWebApi.Models.Account.Account;

namespace MarketPlaceWebApi.Mapper.AutoMapper
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
