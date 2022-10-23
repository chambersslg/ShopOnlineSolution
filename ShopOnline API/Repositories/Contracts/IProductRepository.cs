namespace ShopOnline_API.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<IEnumerable<Product>> GetProducts();
        Task<Product?> GetProductById(int id);
        Task<ProductCategory?> GetCategoryById(int id);
        Task<IEnumerable<Product>> GetProductsByCategory(int categoryId);

    }
}
