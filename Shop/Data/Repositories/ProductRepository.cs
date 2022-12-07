using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;
using Shop.DTO.Product;
using SSC.Data.Repositories;

namespace Shop.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;

        public ProductRepository(DataContext context, IMapper mapper, IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
        }

        public async Task<DbResult<Product>> AddProduct(ProductCreateDTO product)
        {
            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => GetProductByName(product.Name).Result != null, "A product with that name already exists" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            var newProduct = mapper.Map<Product>(product);

            await context.AddAsync(newProduct);
            await context.SaveChangesAsync();

            return DbResult<Product>.CreateSuccess("Product added", newProduct);
        }

        public async Task<DbResult<List<Product>>> GetProducts()
        {
            var productList = await context.Products.ToListAsync();

            return DbResult<List<Product>>.CreateSuccess("Success", productList);
        }

       
        public async Task<DbResult<Product>> DeleteProduct(Guid productId)
        {
            var product = await GetProductById(productId);

            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => product == null, "Product does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            var orders = orderRepository.GetOrdersAssociatedWithProductId(product.Id);

            context.RemoveRange(orders);

            var carts = cartRepository.GetCartsAssociatedWithProductId(product.Id);

            context.RemoveRange(carts);
            context.Remove(product);
            await context.SaveChangesAsync();

            return DbResult<Product>.CreateSuccess("Product has been deleted", product);
        }

        public async Task<DbResult<Product>> EditProduct(ProductUpdateDTO product)
        {
            var productToCheck = await GetProductById(product.Id);

            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => productToCheck == null, "Product does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            mapper.Map(product, productToCheck);

            context.Update(productToCheck);
            await context.SaveChangesAsync();

            return DbResult<Product>.CreateSuccess("Product edited", productToCheck);
        }

        public async Task<DbResult<Product>> GetProductDetails(Guid productId)
        {
            var product = await GetProductById(productId);

            Dictionary<Func<bool>, string> conditions = new Dictionary<Func<bool>, string>
            {
                { () => product == null, "Product does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            return DbResult<Product>.CreateSuccess("Success", product);
        }

        public async Task<DbResult<List<Product>>> SearchProducts(string query)
        {
            if (query == null)
            {
                return DbResult<List<Product>>.CreateSuccess("Success", new List<Product>());
            }

            var searchList = await context.Products
                .Where(x => x.Name.ToLower().Contains(query.ToLower()))
                .ToListAsync();

            return DbResult<List<Product>>.CreateSuccess("Success", searchList);
        }

        private async Task<Product> GetProductById(Guid productId) => await context.Products.FirstOrDefaultAsync(x => x.Id == productId);

        private async Task<Product> GetProductByName(string productName) => await context.Products.FirstOrDefaultAsync(x => x.Name == productName);
    }
}
