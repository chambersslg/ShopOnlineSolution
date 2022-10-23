using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> GetItem(int id);
        Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto itemQtyUpdateDto);
        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartChanged(int totalQty);
    }
}
