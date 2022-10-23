using Newtonsoft.Json;
using ShopOnline_Web.Services.Contracts;
using ShopOnlineModels.Dtos;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline_Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7079/");
        }

        public async Task<CartItemDto> AddItem(CartItemToAddDto itemToAdd)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", itemToAdd);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return default(CartItemDto);
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                } 
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} - Message:{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<CartItemDto> GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                // TODO Fix URI
                var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetCartItems");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return Enumerable.Empty<CartItemDto>().ToList();
                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);
            }
            catch (Exception)
            {
                // Log exception
                throw;
            }
        }

        public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
                var content = new StringContent(jsonRequest,encoding: Encoding.UTF8, "application/json-path+json");
                var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;
            }
            catch (Exception)
            {
                // Log exception
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            // check for subscribers
            if (OnShoppingCartChanged != null)
            {
                // Send event to all subscribers
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }
    }
}
