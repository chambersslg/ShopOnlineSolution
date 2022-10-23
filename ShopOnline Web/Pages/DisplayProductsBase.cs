using Microsoft.AspNetCore.Components;
using ShopOnlineModels.Dtos;

namespace ShopOnline_Web.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
