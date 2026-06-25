using AutoMapper;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.DTOs.Netbox;
using SuiteCoreBackend.Models.Entities;

namespace SuiteCoreBackend.Settings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LdapUser, LdapUserDto>().ReverseMap();
            CreateMap<NetboxRegionResult, NetboxRegionDto>().ReverseMap();
            CreateMap<NetboxRegionResult, NetboxRegionDetailDto>().ReverseMap();
            CreateMap<NetboxIpAddressResult, NetboxIpAddressDto>().ReverseMap();
            CreateMap<NetboxStatusResult, NetboxStatusDto>().ReverseMap();
        }
    }
}
