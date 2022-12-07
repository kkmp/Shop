using Shop.Data.Models;
using Shop.DTO.Order;

namespace Shop.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<DbResult<float>> GetOrderPrice(Guid userId);
        Task<DbResult<List<Order>>> GetOrders(Guid userId);
        ParallelQuery<Order> GetOrdersAssociatedWithProductId(Guid productId);
    }
}
