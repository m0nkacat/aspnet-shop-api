using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApplication5.DTOs;
using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public interface IProductService : ICrudService<ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO> { }

public class ProductService(IRepository<Product> repository, IMapper mapper)
    : CrudService<Product, ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>(repository, mapper), IProductService
{
    protected override string EntityName => "Товар";

    protected override void OnCreating(Product entity, CreateProductDTO dto) => entity.CreatedAt = DateTime.UtcNow;
}

public interface ICategoryService : ICrudService<CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO> { }

public class CategoryService(IRepository<Category> repository, IMapper mapper)
    : CrudService<Category, CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>(repository, mapper), ICategoryService
{
    protected override string EntityName => "Категория";
}

public interface IRoleService : ICrudService<RoleDTO, CreateRoleDTO, UpdateRoleDTO, PatchRoleDTO> { }

public class RoleService(IRepository<Role> repository, IMapper mapper)
    : CrudService<Role, RoleDTO, CreateRoleDTO, UpdateRoleDTO, PatchRoleDTO>(repository, mapper), IRoleService
{
    protected override string EntityName => "Роль";
}

public interface IUserService : ICrudService<UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO> { }

public class UserService(IRepository<User> repository, IMapper mapper, IPasswordHasher<User> passwordHasher)
    : CrudService<User, UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>(repository, mapper), IUserService
{
    protected override string EntityName => "Пользователь";

    protected override void OnCreating(User entity, CreateUserDTO dto) => entity.PasswordHash = passwordHasher.HashPassword(entity, dto.Password);

    protected override void OnUpdating(User entity, UpdateUserDTO dto) => entity.PasswordHash = passwordHasher.HashPassword(entity, dto.Password);

    protected override void OnPatching(User entity, PatchUserDTO dto)
    {
        if (dto.Password is not null)
        {
            entity.PasswordHash = passwordHasher.HashPassword(entity, dto.Password);
        }
    }
}

public interface IOrderService : ICrudService<OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO> { }

public class OrderService(IRepository<Order> repository, IMapper mapper)
    : CrudService<Order, OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>(repository, mapper), IOrderService
{
    protected override string EntityName => "Заказ";

    protected override void OnCreating(Order entity, CreateOrderDTO dto) => entity.OrderDate ??= DateTime.Now;
}

public interface IOrderItemService : ICrudService<OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO> { }

public class OrderItemService(IRepository<OrderItem> repository, IMapper mapper)
    : CrudService<OrderItem, OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>(repository, mapper), IOrderItemService
{
    protected override string EntityName => "Позиция заказа";
}

public interface ICartService : ICrudService<CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO> { }

public class CartService(IRepository<Cart> repository, IMapper mapper)
    : CrudService<Cart, CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>(repository, mapper), ICartService
{
    protected override string EntityName => "Корзина";
}

public interface ICartItemService : ICrudService<CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO> { }

public class CartItemService(IRepository<CartItem> repository, IMapper mapper)
    : CrudService<CartItem, CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>(repository, mapper), ICartItemService
{
    protected override string EntityName => "Позиция корзины";
}
