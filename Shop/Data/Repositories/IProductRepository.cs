using Shop.Data.Models;
using Shop.DTO.Product;

namespace Shop.Data.Repositories
{
    public interface IProductRepository
    {
        Task<DbResult<Product>> AddProduct(ProductCreateDTO product);
        Task<DbResult<List<Product>>> GetProducts();
        Task<DbResult<Product>> DeleteProduct(Guid productId);
        Task<DbResult<Product>> EditProduct(ProductUpdateDTO product);
        Task<DbResult<Product>> GetProductDetails(Guid productId);
        Task<DbResult<List<Product>>> SearchProducts(string query);
    };
}
