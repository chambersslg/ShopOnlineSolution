using Microsoft.AspNetCore.Mvc;
using ShopOnline_API.Extentions;
using ShopOnline_API.Repositories.Contracts;
using ShopOnlineModels.Dtos;

namespace ShopOnline_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetProducts();

                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto();
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
                throw;
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int Id)
        {
            try
            {
                var product = await _productRepository.GetProductById(Id);
                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var category = await _productRepository.GetCategoryById(product.CategoryId);
                    var productDto = product.CovertToDto();
                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await _productRepository.GetCategories();
                if (productCategories != null)
                {
                    var productCategoriesDto = productCategories.ConvertToDto();
                    return Ok(productCategoriesDto);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("{categoryId}/GetProductsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _productRepository.GetProductsByCategory(categoryId);
                if (products != null)
                {
                    var productsDto = products.ConvertToDto();
                    return Ok(productsDto);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }
    }
}
