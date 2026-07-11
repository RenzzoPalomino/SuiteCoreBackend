using AutoMapper;
using SuiteCoreBackend.DTOs.Auth;
using SuiteCoreBackend.DTOs.Monitoring;
using SuiteCoreBackend.DTOs.Netbox;
using SuiteCoreBackend.DTOs.Notification;
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
            CreateMap<NetboxVlanResult, NetboxVlanDto>().ReverseMap();
            CreateMap<NetboxCableResult, NetboxCableDto>().ReverseMap();
            CreateMap<NetboxTerminationResult, NetboxTerminationDto>().ReverseMap();
            CreateMap<NetboxLengthUnitResult, NetboxLengthUnitDto>().ReverseMap();
            CreateMap<NetboxSiteResult, NetboxSiteDto>().ReverseMap();
            CreateMap<NetboxModuleTypeProfileResult, NetboxModuleTypeProfileDto>().ReverseMap();
            CreateMap<NetboxManufacturerResult, NetboxManufacturerDto>().ReverseMap();
            CreateMap<NetboxDeviceRoleResult, NetboxDeviceRoleDto>().ReverseMap();
            CreateMap<NetboxDeviceResult, NetboxDeviceDto>().ReverseMap();
            CreateMap<NetboxDeviceTypeResult, NetboxDeviceTypeDto>().ReverseMap();
            CreateMap<NetboxAirflowResult, NetboxAirflowDto>().ReverseMap();
            CreateMap<NetboxWeightUnitResult, NetboxWeightUnitDto>().ReverseMap();
            CreateMap<NetboxManufacturerNestedResult, NetboxManufacturerNestedDto>().ReverseMap();
            CreateMap<NetboxDeviceRoleNestedResult, NetboxDeviceRoleNestedDto>().ReverseMap();
            CreateMap<NetboxSiteNestedResult, NetboxSiteNestedDto>().ReverseMap();
            CreateMap<NetboxRackNestedResult, NetboxRackNestedDto>().ReverseMap();
            CreateMap<NetboxRackResult, NetboxRackDto>().ReverseMap();
            CreateMap<NetboxRackWidthResult, NetboxRackWidthDto>().ReverseMap();
            CreateMap<NetboxVirtualMachineResult, NetboxVirtualMachineDto>().ReverseMap();
            CreateMap<NetboxStartOnBootResult, NetboxStartOnBootDto>().ReverseMap();
            CreateMap<NetboxClusterNestedResult, NetboxClusterNestedDto>().ReverseMap();
            CreateMap<NetboxClusterResult, NetboxClusterDto>().ReverseMap();
            CreateMap<NetboxClusterTypeNestedResult, NetboxClusterTypeNestedDto>().ReverseMap();
            CreateMap<NetboxPrimaryIpResult, NetboxPrimaryIpDto>().ReverseMap();
            CreateMap<NetboxIpFamilyResult, NetboxIpFamilyDto>().ReverseMap();

            CreateMap<NotificationChannel, NotificationChannelDto>().ReverseMap();
            CreateMap<CreateNotificationChannelDto, NotificationChannel>();
            CreateMap<UpdateNotificationChannelDto, NotificationChannel>();
        }
    }
}
