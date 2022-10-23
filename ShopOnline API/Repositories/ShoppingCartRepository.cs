using Microsoft.EntityFrameworkCore;
using ShopOnline_API.Data;
using ShopOnline_API.Extentions;
using ShopOnline_API.Repositories.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_API.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ShoppingCartRepository(ShopOnlineDbContext context)
        {
            _context = context;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _context.CartItems.AnyAsync(c => c.CartId == cartId
                                                        && c.ProductId == productId);
        }

        public async Task<CartItem> AddCartItem(CartItemToAddDto cartItemToAddDto)
        {
            if (cartItemToAddDto == null) return null;
            // Only add item once
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from p in _context.Products
                                  where p.Id == cartItemToAddDto.ProductId
                                  select cartItemToAddDto.ConvertFromDto()).FirstOrDefaultAsync();
                if (item != null)
                {
                    var result = await _context.CartItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteCartItem(int id)
        {
            var cartItem = await this._context.CartItems.FindAsync(id); //GetCartItem(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return cartItem;
        }

        public async Task<CartItem> GetCartItem(int id)
        {
            return await (from c in _context.Carts
                          join ci in _context.CartItems
                          on c.Id equals ci.CartId
                          where ci.Id == id
                          select new CartItem
                          {
                              Id = ci.Id,
                              ProductId = ci.ProductId,
                              Qty = ci.Qty,
                              CartId = ci.CartId,
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItems(int userId)
        {
            return await (from c in this._context.Carts
                          join ci in _context.CartItems
                          on c.Id equals ci.CartId
                          where c.UserId == userId
                          select new CartItem
                          {
                              Id = ci.Id,
                              ProductId = ci.ProductId,
                              Qty = ci.Qty,
                              CartId = ci.CartId,
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateCartItemQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await _context.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}
