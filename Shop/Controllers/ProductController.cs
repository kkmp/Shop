using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Data.Repositories;
using Shop.DTO.Product;
using SSC.Controllers;

namespace Shop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : CommonController
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(ProductCreateDTO product)
        {
            return await ExecuteForResult(async () => await productRepository.AddProduct(product));
        }

        [AllowAnonymous]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var result = await productRepository.GetProducts();

            return Ok(new { products = mapper.Map<List<ProductGetDTO>>(result.Data) });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("removeProduct/{productId}")]
        public async Task<IActionResult> RemoveProduct(Guid productId)
        {
            return await ExecuteForResult(async () => await productRepository.DeleteProduct(productId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("editProduct")]
        public async Task<IActionResult> EditProduct(ProductUpdateDTO product)
        {
            return await ExecuteForResult(async () => await productRepository.EditProduct(product));
        }

        [HttpGet("details/{productId}")]
        public async Task<IActionResult> Details(Guid productId)
        {
            if (ModelState.IsValid)
            {
                var result = await productRepository.GetProductDetails(productId);

                if (result.Success)
                {
                    return Ok(new { product = mapper.Map<ProductGetDTO>(result.Data) });
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
                var result = await productRepository.SearchProducts(query);
                return Ok(new { products = mapper.Map<List<ProductGetDTO>>(result.Data) });
            }
            return InvalidData();
        }
    }
}
