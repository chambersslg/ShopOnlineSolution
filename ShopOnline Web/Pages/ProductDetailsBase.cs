using Microsoft.AspNetCore.Components;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }
        public ProductDto Product { get; set; }
        public string ErrorMessage { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }


        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {          
                if (cartItemToAddDto != null)
                {
                    var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);
                    ShoppingCartItems.Add(cartItemDto);
                    await CartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }
                NavigationManager.NavigateTo("/ShoppingCart");

            }
            catch (Exception)
            {
                // log exception
            }
        }

        private async Task<ProductDto> GetProductById(int id)
        {
            var productDtos = await ProductsLocalStorageService.GetCollection();
            if (productDtos != null)
            {
                return productDtos.SingleOrDefault(p => p.Id == id);
            }
            return null;
        }
    }
}
