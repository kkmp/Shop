using Shop.Data.Models;

namespace Shop.Data.Repositories
{
    public interface ICartRepository
    {
        Task<DbResult<Cart>> AddProductToCart(Guid productId, Guid issuerId);
        Task<DbResult<int>> GetNumberOfProductsInCart(Guid issuerId);
        Task<DbResult<List<Product>>> GetProductsFromCart(Guid issuerId);
        Task<DbResult<Cart>> RemoveProductFromCart(Guid productId, Guid issuerId);
        ParallelQuery<Cart> GetCartsAssociatedWithProductId(Guid productId);
    }
}
