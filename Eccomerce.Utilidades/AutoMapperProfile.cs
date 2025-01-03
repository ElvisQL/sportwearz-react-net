using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eccomerce.DTO;
using Eccomerce.DTO.Brand;
using Eccomerce.DTO.Category;
using Eccomerce.DTO.DetalleVenta;
using Eccomerce.DTO.Producto;
using Eccomerce.DTO.Role;
using Eccomerce.DTO.User;
using Eccomerce.DTO.Venta;
using Eccomerce.MODELO;

namespace Eccomerce.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, UserSessionDTO>();
            CreateMap<User,UserCreateDTO>();
            CreateMap<UserLoginDTO, User>();
            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Product, ProductoDTO>()
           .ForMember(dest => dest.CategoriesIds,
               opt => opt.MapFrom(src => src.Categories.Select(c => c.CategoryId)))
           .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand));

            CreateMap<ProductoDTO, Product>()
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore());

            CreateMap<DetalleVenta, DetalleVentaDTO>();
            CreateMap<DetalleVentaDTO,DetalleVenta>();

            CreateMap<Venta, VentaDTO>();
            CreateMap<VentaDTO, Venta>();

            CreateMap<Brand, BrandDTO>();
            CreateMap<BrandDTO, Brand>();
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
        }
    }
}
