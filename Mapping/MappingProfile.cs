using AutoMapper;
using WebApplication5.DTOs;
using WebApplication5.Models;

namespace WebApplication5.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);

        CreateMap<Product, ProductDTO>();
        CreateMap<Product, ProductShortDTO>();
        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>();
        CreateMap<PatchProductDTO, Product>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<Category, CategoryDTO>();
        CreateMap<Category, CategoryShortDTO>();
        CreateMap<CreateCategoryDTO, Category>();
        CreateMap<UpdateCategoryDTO, Category>();
        CreateMap<PatchCategoryDTO, Category>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<Role, RoleDTO>();
        CreateMap<Role, RoleShortDTO>();
        CreateMap<CreateRoleDTO, Role>();
        CreateMap<UpdateRoleDTO, Role>();
        CreateMap<PatchRoleDTO, Role>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<User, UserDTO>();
        CreateMap<User, UserShortDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<UpdateUserDTO, User>();
        CreateMap<PatchUserDTO, User>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<Order, OrderDTO>();
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<UpdateOrderDTO, Order>();
        CreateMap<PatchOrderDTO, Order>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<OrderItem, OrderItemDTO>();
        CreateMap<CreateOrderItemDTO, OrderItem>();
        CreateMap<UpdateOrderItemDTO, OrderItem>();
        CreateMap<PatchOrderItemDTO, OrderItem>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<Cart, CartDTO>();
        CreateMap<CreateCartDTO, Cart>();
        CreateMap<UpdateCartDTO, Cart>();
        CreateMap<PatchCartDTO, Cart>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<CartItem, CartItemDTO>();
        CreateMap<CreateCartItemDTO, CartItem>();
        CreateMap<UpdateCartItemDTO, CartItem>();
        CreateMap<PatchCartItemDTO, CartItem>().ForAllMembers(o => o.Condition((_, _, src) => src != null));

        CreateMap<Reservation, ReservationDTO>()
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.CanceledAt == null));
        CreateMap<CreateReservationDTO, Reservation>();
    }
}
