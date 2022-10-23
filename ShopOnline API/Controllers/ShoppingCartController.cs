using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline_API.Extentions;
using ShopOnline_API.Repositories.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("{userId}/GetCartItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await _shoppingCartRepository.GetCartItems(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }
                var products = await _productRepository.GetProducts();

                if (products == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var cartItemsDto = cartItems.ConvertToDto(products);
                return Ok(cartItemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.GetCartItem(id);
                if (cartItem == null)
                {
                    return NotFound();
                }
                var product = await _productRepository.GetProductById(cartItem.ProductId);

                if (product == null) return NotFound();

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemDto)
        {
            try
            {
                var newCartItem = await _shoppingCartRepository.AddCartItem(cartItemDto);

                if (newCartItem == null) return NoContent();

                var product = await _productRepository.GetProductById(newCartItem.ProductId);

                if (product == null)
                {
                    throw new Exception($"Not able to retrieve product {cartItemDto.ProductId}");
                }

                var newCartItemDto = newCartItem.ConvertToDto(product);

                return CreatedAtAction(nameof(GetItem), new {id = newCartItemDto.Id}, newCartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var deleted = await _shoppingCartRepository.DeleteCartItem(id);
                if (deleted == null)
                {
                    return NotFound();
                }
                var product = await _productRepository.GetProductById(deleted.ProductId);
                if (product == null) return NotFound();

                return Ok(deleted.ConvertToDto(product));
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Patch partially updates the resource
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await _shoppingCartRepository.UpdateCartItemQty(id, cartItemQtyUpdateDto);
                if (cartItem == null) return NotFound();
                var product = await _productRepository.GetProductById(cartItem.ProductId);
                var cartItemoDto = cartItem.ConvertToDto(product);

                return Ok(cartItemoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
