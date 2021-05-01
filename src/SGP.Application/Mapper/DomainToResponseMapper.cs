using AutoMapper;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Entities.UserAggregate;

namespace SGP.Application.Mapper
{
    public class DomainToResponseMapper : Profile
    {
        public DomainToResponseMapper()
        {
            CreateMap<City, CityResponse>(MemberList.Destination);
            CreateMap<User, UserResponse>(MemberList.Destination)
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
        }
    }
}