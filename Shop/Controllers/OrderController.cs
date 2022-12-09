using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.UnitOfWork;
using Shop.DTO.Order;
using SSC.Controllers;
namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : CommonController
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("orderPrice")]
        public async Task<IActionResult> OrderPrice()
        {
            var issuerId = GetUserId();
            var result = await unitOfWork.OrderRepository.GetOrderPrice(issuerId);

            if (result.Success)
            {
                return Ok(new { orderPrice = result.Data });
            }
            else
            {
                return BadRequestErrorMessage(result.Message);
            }
        }

        [HttpPost("takeOrder")]
        public async Task<IActionResult> TakeOrder(OrderCreateDTO order)
        {
            return await ExecuteForResult(async () => await unitOfWork.OrderRepository.TakeOrder(order, GetUserId()));
        }


        [HttpGet("myOrders")]
        public async Task<IActionResult> MyOrders()
        {
            var issuerId = GetUserId();
            var result = await unitOfWork.OrderRepository.GetOrders(issuerId);

            if (result.Success)
            {
                return Ok(new { myOrders = unitOfWork.Mapper.Map<List<OrderGetDTO>>(result.Data) });
            }
            else
            {
                return BadRequestErrorMessage(result.Message);
            }
        }
    }
}
