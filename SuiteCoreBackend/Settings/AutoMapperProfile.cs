using AutoMapper;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Settings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LdapUser, LdapUserDto>().ReverseMap();
        }
    }
}
