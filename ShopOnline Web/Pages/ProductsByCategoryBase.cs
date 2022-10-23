using Microsoft.AspNetCore.Components;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Pages
{
    public class ProductsByCategoryBase : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public string CategoryName { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Products = await GetProductCollectionByCategoryId(categoryId: CategoryId);

                if (Products != null && Products.Count() > 0)
                {
                    var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);
                    if (productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
        {
            var products =  await ProductsLocalStorageService.GetCollection();
            if (products != null)
            {
                return products.Where(x => x.CategoryId == categoryId);
            }
            else
            {
                return await ProductService.GetProductsByCategory(categoryId);
            }
        }
    }
}
