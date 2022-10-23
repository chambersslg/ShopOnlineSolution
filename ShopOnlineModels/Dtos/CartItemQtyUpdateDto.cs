using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineModels.Dtos
{
    public class CartItemQtyUpdateDto
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public int Qty { get; set; }
    }
}
