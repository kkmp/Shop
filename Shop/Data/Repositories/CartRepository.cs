using Microsoft.EntityFrameworkCore;
using Shop.Data.Models;
using SSC.Data.Repositories;

namespace Shop.Data.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly DataContext context;
        private readonly IUserRepository userRepository;

        public CartRepository(DataContext context, IUserRepository userRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
        }

        public async Task<DbResult<Cart>> AddProductToCart(Guid productId, Guid issuerId)
        {
            //productRepo
            var product = await GetProductById(productId);
            var user = await userRepository.GetUserById(issuerId);

            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => user == null, "User does not exist" },
                { () => product == null, "Product does not exist" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            Cart cart = new Cart
            {
                Product = product,
                User = user
            };

            await context.AddAsync(cart);
            await context.SaveChangesAsync();

            return DbResult<Cart>.CreateSuccess("Item added to cart", cart);
        }

        public async Task<DbResult<int>> GetNumberOfProductsInCart(Guid issuerId)
        {
            if (await userRepository.GetUserById(issuerId) == null)
            {
                return DbResult<int>.CreateFail("User does not exist");
            }

            var itemsNumber = context.Carts.Where(x => x.UserId == issuerId.ToString()).Count();

            return DbResult<int>.CreateSuccess("Success", itemsNumber);
        }

        public async Task<DbResult<List<Product>>> GetProductsFromCart(Guid issuerId)
        {
            if (await userRepository.GetUserById(issuerId) == null)
            {
                return DbResult<List<Product>>.CreateFail("User does not exist");
            }

            var cartList = await context.Carts.Where(x => x.UserId == issuerId.ToString()).Select(x => x.Product).ToListAsync();

            return DbResult<List<Product>>.CreateSuccess("Success", cartList);
        }

        public async Task<DbResult<Cart>> RemoveProductFromCart(Guid productId, Guid issuerId)
        {
            var cart = await context.Carts.Where(x => x.UserId == issuerId.ToString()).FirstOrDefaultAsync(x => x.ProductId == productId);

            //productRepo
            var conditions = new Dictionary<Func<bool>, string>
            {
                { () => userRepository.GetUserById(issuerId).Result == null, "User does not exist" },
                { () => GetProductById(productId).Result == null, "Product does not exist" },
                { () => cart == null, "There is nothing in the cart" }
            };

            var result = Validate(conditions);
            if (result != null)
            {
                return result;
            }

            context.Remove(cart);
            await context.SaveChangesAsync();

            return DbResult<Cart>.CreateSuccess("Item deleted from the cart", cart);
        }

        public ParallelQuery<Cart> GetCartsAssociatedWithProductId(Guid productId)
        {
            return context.Carts
                .Include(x => x.Product)
                .AsParallel()
                .Where(x => x.ProductId == productId);
        }

        private async Task<Product> GetProductById(Guid productId)
        {
            return await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        }
    }
}
