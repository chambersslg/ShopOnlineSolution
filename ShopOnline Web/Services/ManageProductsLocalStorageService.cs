using Blazored.LocalStorage;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;
        private const string _productKey = "ProductCollection";
        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, 
                                                    IProductService productService)
        {
            _localStorageService = localStorageService;
            _productService = productService;
        }
        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<ProductDto>>(_productKey) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(_productKey);
        }

        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await _productService.GetProducts();
            if (productCollection != null)
            {
                await _localStorageService.SetItemAsync(_productKey, productCollection);
            }
            return productCollection;
        }
    }
}
