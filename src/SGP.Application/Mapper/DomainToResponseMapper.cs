using AutoMapper;
using SGP.Application.Responses;
using SGP.Domain.Entities;

namespace SGP.Application.Mapper
{
    public class DomainToResponseMapper : Profile
    {
        public DomainToResponseMapper()
        {
            CreateMap<Cidade, CidadeResponse>(MemberList.Destination);
            CreateMap<Usuario, UsuarioResponse>(MemberList.Destination)
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
        }
    }
}