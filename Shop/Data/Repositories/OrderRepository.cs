﻿using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;
using Shop.Data.UnitOfWork;
using Shop.DTO.Order;
using SSC.Data.Repositories;

namespace Shop.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IUnitOfWork unitOfWork;

        public OrderRepository(DataContext context, IUnitOfWork unitOfWork)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
        }

        public async Task<DbResult<float>> GetOrderPrice(Guid userId)
        {
            if (await unitOfWork.UserRepository.GetUserById(userId) == null)
            {
                return DbResult<float>.CreateFail("User does not exist");
            }

            if (await unitOfWork.CartRepository.IsUsersCartEmpty(userId.ToString()))
            {
                return DbResult<float>.CreateFail("Cart is empty");
            }

            var price = await context.Carts.Where(x => x.UserId == userId.ToString()).Select(x => x.Product).SumAsync(x => x.Price);

            return DbResult<float>.CreateSuccess("Success", price);
        }

        public async Task<DbResult<List<Cart>>> TakeOrder(OrderCreateDTO order, Guid userId)
        {
            var carts = await unitOfWork.CartRepository.GetCartsAssociatedWithUserId(userId);

            if (await unitOfWork.UserRepository.GetUserById(userId) == null)
            {
                return DbResult<List<Cart>>.CreateFail("User does not exist");
            }

            if (!carts.Any())
            {
                return DbResult<List<Cart>>.CreateFail("Cart is empty");
            }

            foreach (var cart in carts)
            {
                var newOrder = MapCartToOrder(order, cart);

                await context.AddAsync(newOrder);
                context.Remove(cart);
            }

            await context.SaveChangesAsync();

            return DbResult<List<Cart>>.CreateSuccess("Order has been realized", carts);
        }

        public async Task<DbResult<List<Order>>> GetOrders(Guid userId)
        {
            if (await unitOfWork.UserRepository.GetUserById(userId) == null)
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
            var newOrder = unitOfWork.Mapper.Map<Order>(order);

            newOrder.Product = cart.Product;
            newOrder.User = cart.User;
            newOrder.Status = "in progress";
            newOrder.PaymentStatus = "in progress";

            return newOrder;
        }
    }
}
