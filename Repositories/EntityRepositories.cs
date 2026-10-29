using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Repositories;

public class ProductRepository(ShopDbContext context) : Repository<Product>(context)
{
    protected override IQueryable<Product> WithDetails(IQueryable<Product> query) =>
        query.Include(p => p.Category);
}

public class CategoryRepository(ShopDbContext context) : Repository<Category>(context)
{
    protected override IQueryable<Category> WithDetails(IQueryable<Category> query) =>
        query.Include(c => c.Products);
}

public class RoleRepository(ShopDbContext context) : Repository<Role>(context)
{
    protected override IQueryable<Role> WithDetails(IQueryable<Role> query) =>
        query.Include(r => r.Users);
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);

    Task<Role?> GetRoleByNameAsync(string roleName);
}

public class UserRepository(ShopDbContext context) : Repository<User>(context), IUserRepository
{
    protected override IQueryable<User> WithDetails(IQueryable<User> query) =>
        query.Include(u => u.Role);

    public Task<User?> GetByUsernameAsync(string username) =>
        WithDetails(Set).FirstOrDefaultAsync(u => u.Username == username);

    public Task<Role?> GetRoleByNameAsync(string roleName) =>
        Context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
}

public class OrderRepository(ShopDbContext context) : Repository<Order>(context)
{
    protected override IQueryable<Order> WithDetails(IQueryable<Order> query) =>
        query.Include(o => o.User)
             .Include(o => o.OrderItems).ThenInclude(i => i.Product);
}

public class OrderItemRepository(ShopDbContext context) : Repository<OrderItem>(context)
{
    protected override IQueryable<OrderItem> WithDetails(IQueryable<OrderItem> query) =>
        query.Include(i => i.Product);
}

public class CartRepository(ShopDbContext context) : Repository<Cart>(context)
{
    protected override IQueryable<Cart> WithDetails(IQueryable<Cart> query) =>
        query.Include(c => c.User)
             .Include(c => c.CartItems).ThenInclude(i => i.Product);
}

public class CartItemRepository(ShopDbContext context) : Repository<CartItem>(context)
{
    protected override IQueryable<CartItem> WithDetails(IQueryable<CartItem> query) =>
        query.Include(i => i.Product);
}
