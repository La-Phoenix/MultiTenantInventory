
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
            CreateMap<CreateTenantDto, Tenant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.AdminFirstName} {src.AdminLastName}"))
                .ForMember(dest => dest.Subdomain, opt => opt.MapFrom(src => src.Subdomain.ToLower()))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerUser, opt => opt.Ignore()) // OwnerUser will be set later
                .ForMember(dest => dest.OwnerUserId, opt => opt.Ignore()); // OwnerUserId will be set later

            CreateMap<CreateTenantDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AdminEmail))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.AdminFirstName} {src.AdminLastName}"))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.AdminPassword)))
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Tenant, opt => opt.Ignore());

        }
    }
}
