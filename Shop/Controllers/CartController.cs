using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.UnitOfWork;
using Shop.DTO;
using SSC.Controllers;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : CommonController
    {
        private readonly IUnitOfWork unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart(IdDTO productId)
        {
            return await ExecuteForResult(async () => await unitOfWork.CartRepository.AddProductToCart(productId.Id.Value, GetUserId()));
        }

        [HttpGet("itemsNumberInCart")]
        public async Task<IActionResult> ItemsNumberInCart()
        {
            return await ExecuteForResult(async () => await unitOfWork.CartRepository.GetNumberOfProductsInCart(GetUserId()));
        }

        [HttpGet("cartList")]
        public async Task<IActionResult> CartList()
        {
            var issuerId = GetUserId();
            var result = await unitOfWork.CartRepository.GetProductsFromCart(issuerId);

            if (result.Success)
            {
                return Ok(new { cartList = result.Data });
            }
            else
            {
                return BadRequestErrorMessage(result.Message);
            }
        }

        [HttpDelete("removeFromCart/{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            return await ExecuteForResult(async () => await unitOfWork.CartRepository.RemoveProductFromCart(productId, GetUserId()));
        }
    }
}
