using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.Repositories;
using Shop.DTO.Order;
using SSC.Controllers;
namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : CommonController
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        [HttpGet("orderPrice")]
        public async Task<IActionResult> OrderPrice()
        {
            var issuerId = GetUserId();
            var result = await orderRepository.GetOrderPrice(issuerId);

            if (result.Success)
            {
                return Ok(new { orderPrice = result.Data });
            }
            else
            {
                return BadRequestErrorMessage(result.Message);
            }
        }

        [HttpGet("myOrders")]
        public async Task<IActionResult> MyOrders()
        {
            var issuerId = GetUserId();
            var result = await orderRepository.GetOrders(issuerId);

            if (result.Success)
            {
                return Ok(new { myOrders = mapper.Map<List<OrderGetDTO>>(result.Data) });
            }
            else
            {
                return BadRequestErrorMessage(result.Message);
            }
        }
    }
}
