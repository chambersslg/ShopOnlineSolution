using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Pages
{
    public class CheckoutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();

                if (ShoppingCartItems != null)
                {
                    Guid orderGuid = Guid.NewGuid();
                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"O_{TestItem.UserId}_{orderGuid}";

                }
            }
            catch (Exception)
            {
                // Log error
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
