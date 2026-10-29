using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Auth;
using WebApplication5.DTOs;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Route("api/categories")]
public class CategoriesController(ICategoryService service)
    : CrudController<CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>(service);

[Route("api/roles")]
[Authorize(Policy = Policies.AdminOnly)]
public class RolesController(IRoleService service)
    : CrudController<RoleDTO, CreateRoleDTO, UpdateRoleDTO, PatchRoleDTO>(service);

[Route("api/users")]
[Authorize(Policy = Policies.AdminOnly)]
public class UsersController(IUserService service)
    : CrudController<UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>(service);

[Route("api/orders")]
[Authorize(Policy = Policies.AdminOnly)]
public class OrdersController(IOrderService service)
    : CrudController<OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>(service);

[Route("api/order-items")]
[Authorize(Policy = Policies.AdminOnly)]
public class OrderItemsController(IOrderItemService service)
    : CrudController<OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>(service);

[Route("api/carts")]
[Authorize(Policy = Policies.AdminOnly)]
public class CartsController(ICartService service)
    : CrudController<CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>(service);

[Route("api/cart-items")]
[Authorize(Policy = Policies.AdminOnly)]
public class CartItemsController(ICartItemService service)
    : CrudController<CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>(service);
