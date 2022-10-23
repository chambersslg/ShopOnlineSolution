using Microsoft.AspNetCore.Components;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public string ErrorMessage { get; set; }


        // Bazor LifeCycle Event
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ClearLocalStorage();

                Products = await ProductsLocalStorageService.GetCollection();
                var shoppingCartItems = await CartItemsLocalStorageService.GetCollection();
                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
           
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory() {
            return from p in Products
            group p by p.CategoryId into prodByCatGroup
            orderby prodByCatGroup.Key
            select prodByCatGroup;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDtos)
        {
            return groupedProductDtos.FirstOrDefault(pg => pg.CategoryId == groupedProductDtos.Key).CategoryName;
        }

        private async Task ClearLocalStorage()
        {
            await ProductsLocalStorageService.RemoveCollection();
            await CartItemsLocalStorageService.RemoveCollection();
        }

    }
}
