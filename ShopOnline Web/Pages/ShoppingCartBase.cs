using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        public string TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);

            // Rerender apon deletion 
            RemoveCartItem(id);
            CartChanged();
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            if (qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto
                {
                    CartItemId = id,
                    Qty = qty
                };

                var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);
                UpdateItemTotalPrice(returnedUpdateItemDto);
                CartChanged();

                await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);
            }
            else
            {
                var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);
                if (item != null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }

        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
        }


        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item!= null)
            {
                item.TotalPrice = item.Price * item.Qty;
            }

            await CartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }
        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
        }
        private async Task RemoveCartItem(int id)
        {
            var cartItem = GetCartItem(id);

            ShoppingCartItems.Remove(cartItem);

            await CartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        private void CalculateCartSummaryTotal()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }
        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C"); 
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(p => p.Qty);
        }
        private void CartChanged()
        {
            CalculateCartSummaryTotal();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity); 
        }

    }
}
