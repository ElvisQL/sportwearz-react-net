
using AutoMapper;

using Eccomerce.DTO.Brand;
using Eccomerce.DTO.Cart;
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
            // Mapeos de User
            CreateMap<User, UserSessionDTO>()
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart));
                
            CreateMap<User, UserCreateDTO>();
            CreateMap<UserLoginDTO, User>();
            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

            // Mapeos de Cart y CartItem
            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));
            // Mapeo CartDTO -> Cart
            CreateMap<CartDTO, Cart>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CartItems, opt => opt.Ignore());


            // Mapeo CartItemDTO -> CartItem
            CreateMap<CartItemDTO, CartItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())  // Ignorar navegación
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
                

            // Mapeos de Category
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            // Mapeos de Product
            CreateMap<Product, ProductoDTO>()
                .ForMember(dest => dest.CategoriesIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.CategoryId)));
                

            CreateMap<ProductoDTO, Product>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.IdProductoNavigation.ProductName))
                .ForMember(dest => dest.ImagenUrl, opt => opt.MapFrom(src => src.IdProductoNavigation.ImageUrl));

            CreateMap<DetalleVentaDTO, DetalleVenta>()
                .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => src.IdProducto)) 
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdVentaNavigation, opt => opt.Ignore());

            // Mapeos de Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(dest => dest.DetalleVenta, opt => opt.MapFrom(src => src.DetalleVenta));




            CreateMap<VentaDTO, Venta>()
                .ForMember(dest => dest.DetalleVenta, opt => opt.MapFrom(src => src.DetalleVenta)) 
                .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore());


            // Mapeos de Brand
            CreateMap<Brand, BrandDTO>();
            CreateMap<BrandDTO, Brand>();

            // Mapeos de Role
            CreateMap<Role, RoleDTO>();
            CreateMap<RoleDTO, Role>();
        }
    }
}
