using AutoMapper;
using SGP.Application.Responses;
using SGP.Domain.Entities;

namespace SGP.Application.Mapper
{
    public class DomainToResponseMapper : Profile
    {
        public DomainToResponseMapper()
        {
            CreateMap<Cidade, CidadeResponse>(MemberList.Destination)
                .ForMember(dest => dest.Regiao, opt => opt.MapFrom(src => src.Estado.Regiao.Nome))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.Nome))
                .ForMember(dest => dest.Uf, opt => opt.MapFrom(src => src.Estado.Uf));

            CreateMap<Estado, EstadoResponse>(MemberList.Destination)
                .ForMember(dest => dest.Regiao, opt => opt.MapFrom(src => src.Regiao.Nome));
        }
    }
}