using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shop.Data.Repositories;
using Shop.Services;

namespace Shop.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        public IRoleRepository RoleRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public ICartRepository CartRepository { get; set; }
        public UserManager<IdentityUser> UserManager { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public IMapper Mapper { get; set; }

        public UnitOfWork(DataContext context, IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            Mapper = mapper;
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            UserRepository = new UserRepository(context, this);
            RoleRepository = new RoleRepository(context);
            ProductRepository = new ProductRepository(context, this);
            OrderRepository = new OrderRepository(context, this);
            CartRepository = new CartRepository(context, this);
        }
    }
}
