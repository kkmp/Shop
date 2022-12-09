using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.UnitOfWork;
using Shop.DTO.Product;
using SSC.Controllers;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : CommonController
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(ProductCreateDTO product)
        {
            return await ExecuteForResult(async () => await unitOfWork.ProductRepository.AddProduct(product));
        }

        [AllowAnonymous]
        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await unitOfWork.ProductRepository.GetProducts();

            return Ok(new { products = unitOfWork.Mapper.Map<List<ProductGetDTO>>(result.Data) });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("removeProduct/{productId}")]
        public async Task<IActionResult> RemoveProduct(Guid productId)
        {
            return await ExecuteForResult(async () => await unitOfWork.ProductRepository.DeleteProduct(productId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("editProduct")]
        public async Task<IActionResult> EditProduct(ProductUpdateDTO product)
        {
            return await ExecuteForResult(async () => await unitOfWork.ProductRepository.EditProduct(product));
        }

        [HttpGet("details/{productId}")]
        public async Task<IActionResult> Details(Guid productId)
        {
            if (ModelState.IsValid)
            {
                var result = await unitOfWork.ProductRepository.GetProductDetails(productId);

                if (result.Success)
                {
                    return Ok(new { product = unitOfWork.Mapper.Map<ProductGetDTO>(result.Data) });
                }
                else
                {
                    return BadRequestErrorMessage(result.Message);
                }
            }
            return InvalidData();
        }

        [AllowAnonymous]
        [HttpGet("search/{query}")]
        public async Task<IActionResult> Search(string query)
        {
            if (ModelState.IsValid)
            {
                var result = await unitOfWork.ProductRepository.SearchProducts(query);
                return Ok(new { products = unitOfWork.Mapper.Map<List<ProductGetDTO>>(result.Data) });
            }
            return InvalidData();
        }
    }
}
