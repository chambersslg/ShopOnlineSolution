using Microsoft.EntityFrameworkCore;
using ShopOnline_API.Data;
using ShopOnline_API.Repositories.Contracts;

namespace ShopOnline_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ProductRepository(ShopOnlineDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _context.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<ProductCategory?> GetCategoryById(int id)
        {
            return await _context.ProductCategories.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
                            .Include(p => p.ProductCategory)
                            .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products
                                .Include(p => p.ProductCategory)
                                .ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                        .Include(p => p.ProductCategory)
                        .Where(x => x.CategoryId == categoryId)
                        .ToListAsync();
        }
    }
}
