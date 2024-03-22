using Accounting.Application.Service.Product;
using Accounting.Application.Service.Product.Dtos;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Create-Product")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequestDto request)
        {
            var response = await _productService.CreateProduct(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Delete-Product")]
        public async Task<IActionResult> DeleteOrder([FromBody] Guid id)
        {
            var response = await _productService.DeleteProduct(id).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("GetAll-Product")]
        public async Task<IActionResult> GetAllProducts([FromBody] GetAllProductRequestDto request)
        {
            var response = await _productService.GetAllProduct(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Update-Product")]
        public async Task<IActionResult> UpdateProducts([FromBody] ProductUpdateRequestDto request)
        {
            var response = await _productService.UpdateProduct(request).ConfigureAwait(false);
            if (!response.IsSuccesfull)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
