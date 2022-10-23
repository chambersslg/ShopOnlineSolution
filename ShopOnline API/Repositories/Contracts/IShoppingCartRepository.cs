using ShopOnlineModels.Dtos;

namespace ShopOnline_API.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddCartItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItem> UpdateCartItemQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);
        Task<CartItem> DeleteCartItem(int id);
        Task<CartItem> GetCartItem(int id);
        Task<IEnumerable<CartItem>> GetCartItems(int userId);
    }
}
