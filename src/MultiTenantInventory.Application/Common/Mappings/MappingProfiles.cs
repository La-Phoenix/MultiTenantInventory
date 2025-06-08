
using AutoMapper;
using MultiTenantInventory.Application.DTOs;
using MultiTenantInventory.Domain.Entities;

namespace MultiTenantInventory.Application.Common.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Tenant, TenantDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
