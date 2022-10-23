using Blazored.LocalStorage;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IShoppingCartService _shoppingCartService;
        private const string _cartItemsKey = "CartItemsCollection";
        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService,
                                                    IShoppingCartService cartService)
        {
            _localStorageService = localStorageService;
            _shoppingCartService = cartService;
        }
        public async Task<List<CartItemDto>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<List<CartItemDto>>(_cartItemsKey) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(_cartItemsKey);
        }

        public async Task SaveCollection(List<CartItemDto> cartItems)
        {
           await _localStorageService.SetItemAsync(_cartItemsKey, cartItems);
        }

        private async Task<List<CartItemDto>> AddCollection()
        {
            var cartItemsCollection = await _shoppingCartService.GetItems(TestItem.UserId);
            if (cartItemsCollection != null)
            {
                await _localStorageService.SetItemAsync(_cartItemsKey, cartItemsCollection);
            }
            return cartItemsCollection;
        }
    }
}
