using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Repositories;

namespace GuitarShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ILogger<ProductsController> logger,
        ProductRepo repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await repo.GetAllAsync();

                return Ok(products.Select(p => p.ProductName));
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Products");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await repo.FindByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Product by Id.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
