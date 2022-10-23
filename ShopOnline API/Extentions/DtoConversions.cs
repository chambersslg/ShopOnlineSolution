using ShopOnlineModels.Dtos;

namespace ShopOnline_API.Extentions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from p in products
                    select new ProductDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageURL = p.ImageURL,
                        Price = p.Price,
                        Qty = p.Qty,
                        CategoryId = p.CategoryId,
                        CategoryName = p.ProductCategory.Name
                    }).ToList();
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from ci in cartItems
                    join p in products
                    on ci.ProductId equals p.Id
                    select new CartItemDto
                    {
                        Id = ci.Id,
                        ProductId = p.Id,
                        ProductName = p.Name,
                        ProductDescription = p.Description,
                        ProductImageURL = p.ImageURL,
                        Price = p.Price,
                        CartId = ci.CartId,
                        Qty = ci.Qty,
                        TotalPrice = p.Price * ci.Qty
                    }).ToList();
        }


        public static ProductDto CovertToDto(this Product p)
        {
            return new ProductDto()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageURL = p.ImageURL,
                        Price = p.Price,
                        Qty = p.Qty,
                        CategoryId = p.CategoryId,
                        CategoryName = p.ProductCategory.Name,            
                    };
        }

        public static CartItemDto ConvertToDto(this CartItem ci, Product p)
        {
            return new CartItemDto
            {
                Id = ci.Id,
                ProductId = p.Id,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductImageURL = p.ImageURL,
                Price = p.Price,
                CartId = ci.CartId,
                Qty = ci.Qty,
                TotalPrice = p.Price * ci.Qty
            };
        }

        public static ProductCategoryDto ConvertToDto(this ProductCategory pc)
        {
            return new ProductCategoryDto
            {
                Id = pc.Id,
                Name = pc.Name,
                IconCSS = pc.IconCSS
            };
        }

        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory>? productCategories)
        {
            return (from pc in productCategories
                    select new ProductCategoryDto
                    {
                        Id = pc.Id,
                        Name = pc.Name,
                        IconCSS = pc.IconCSS
                    }).ToList();
        }

        public static CartItem CovertFromDto(this CartItemDto c)
        {
            return new CartItem()
            {
                CartId = c.CartId,
                Id = c.Id,
                ProductId = c.ProductId,
                Qty = c.Qty
            };
        }

        public static CartItem ConvertFromDto(this CartItemToAddDto c)
        {
            return new CartItem()
            {
                CartId = c.CartId,
                ProductId = c.ProductId,
                Qty = c.Qty
            };
        }

    }
}
