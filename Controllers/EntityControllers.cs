using Microsoft.AspNetCore.Mvc;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Route("api/categories")]
public class CategoriesController(ICategoryService service)
    : CrudController<CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>(service);

[Route("api/roles")]
public class RolesController(IRoleService service)
    : CrudController<RoleDTO, CreateRoleDTO, UpdateRoleDTO, PatchRoleDTO>(service);

[Route("api/users")]
public class UsersController(IUserService service)
    : CrudController<UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>(service);

[Route("api/orders")]
public class OrdersController(IOrderService service)
    : CrudController<OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>(service);

[Route("api/order-items")]
public class OrderItemsController(IOrderItemService service)
    : CrudController<OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>(service);

[Route("api/carts")]
public class CartsController(ICartService service)
    : CrudController<CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>(service);

[Route("api/cart-items")]
public class CartItemsController(ICartItemService service)
    : CrudController<CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>(service);
