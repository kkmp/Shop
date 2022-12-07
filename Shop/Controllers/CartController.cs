using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.Repositories;
using Shop.DTO;
using SSC.Controllers;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : CommonController
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart(IdDTO productId)
        {
            return await ExecuteForResult(async () => await cartRepository.AddProductToCart(productId.Id.Value, GetUserId()));
        }

        [HttpGet("itemsNumberInCart")]
        public async Task<IActionResult> ItemsNumberInCart()
        {
            return await ExecuteForResult(async () => await cartRepository.GetNumberOfProductsInCart(GetUserId()));
        }

        [HttpGet("cartList")]
        public async Task<IActionResult> CartList()
        {
            var issuerId = GetUserId();
            var result = await cartRepository.GetProductsFromCart(issuerId);

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
            return await ExecuteForResult(async () => await cartRepository.RemoveProductFromCart(productId, GetUserId()));
        }
    }
}
