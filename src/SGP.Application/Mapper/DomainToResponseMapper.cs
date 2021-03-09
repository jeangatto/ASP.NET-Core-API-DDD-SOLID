using AutoMapper;
using SGP.Application.Responses;
using SGP.Domain.Entities;

namespace SGP.Application.Mapper
{
    public class DomainToResponseMapper : Profile
    {
        public DomainToResponseMapper()
        {
            CreateMap<Pais, PaisResponse>(MemberList.Destination);
        }
    }
}
