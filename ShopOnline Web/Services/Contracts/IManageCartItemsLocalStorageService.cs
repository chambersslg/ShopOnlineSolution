using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<List<CartItemDto>> GetCollection();
        Task SaveCollection(List<CartItemDto> cartItems);
        Task RemoveCollection();
    }
}
