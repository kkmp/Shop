using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;
using Shop.DTO.Order;
using SSC.Data.Repositories;

namespace Shop.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public OrderRepository(DataContext context, IMapper mapper, IUserRepository userRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<DbResult<float>> GetOrderPrice(Guid userId)
        {
            if (await userRepository.GetUserById(userId) == null)
            {
                return DbResult<float>.CreateFail("User does not exist");
            }

            var price = await context.Carts.Where(x => x.UserId == userId.ToString()).Select(x => x.Product).SumAsync(x => x.Price);

            return DbResult<float>.CreateSuccess("Success", price);
        }

        public async Task<DbResult<List<Order>>> GetOrders(Guid userId)
        {
            if (await userRepository.GetUserById(userId) == null)
            {
                return DbResult<List<Order>>.CreateFail("User does not exist");
            }

            var orderList = await context.Orders
                .Include(x => x.Product)
                .Where(x => x.UserId == userId.ToString())
                .ToListAsync();

            return DbResult<List<Order>>.CreateSuccess("Success", orderList);
        }

        public ParallelQuery<Order> GetOrdersAssociatedWithProductId(Guid productId)
        {
            return context.Orders
                .Include(x => x.Product)
                .AsParallel()
                .Where(x => x.ProductId == productId);
        }

        private Order MapCartToOrder(OrderCreateDTO order, Cart cart)
        {
            var newOrder = mapper.Map<Order>(order);

            newOrder.Product = cart.Product;
            newOrder.User = cart.User;
            newOrder.Status = "in progress";
            newOrder.PaymentStatus = "in progress";

            return newOrder;
        }
    }
}
