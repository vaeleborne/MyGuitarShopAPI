using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace GuitarShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        ILogger<ProductsController> logger,
        ProductRepo repo) : ControllerBase
    {
        #region CREATE_ROUTES
        [HttpPost("{categoryId, productCode, productName, description, listPrice, discountPercent}")]
        public async Task<IActionResult> InsertAsync(int categoryId, string productCode, string productName, string description, decimal listPrice, decimal discountPercent)
        {
            try
            {
                var product = new ProductEntity()
                {
                    ProductID = 0, //Doesn't matter, sql will actually assign this
                    CategoryID = categoryId,
                    ProductName = productName,
                    ProductCode = productCode,
                    ListPrice = listPrice,
                    DiscountPercent = discountPercent,
                    Description = description,
                    DateAdded = DateTime.UtcNow
                };
                await repo.InsertAsync(product);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Inserting a new product.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region READ_ROUTES
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

        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            try
            {
                var product = await repo.FindByUniqueAsync(name);
                return Ok(product);
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Product by Name.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region UPDATE_ROUTES
        //TODO: Add other update fields
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProductDTO dto)
        {
            if (dto == null) return BadRequest("No Data Given");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.ProductID == null || dto.ProductID != id) return BadRequest("Product ID in URL and Body Must Match.");
            try
            {
                if (dto.ProductID != null)
                {
                    var product = await repo.FindByIdAsync(dto.ProductID.Value);

                    if (product == null)  return NotFound();


                    product.ProductName = dto.ProductName ?? product.ProductName;
                    product.ListPrice = dto.ListPrice!.Value;
                    product.DiscountPercent = dto.DiscountPercent!.Value;

                    await repo.UpdateAsync(product.ProductID, product);
                    return Ok(product);
                }
                else
                {

                    throw new Exception("Product Not Found.");
                }
              
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Updating product {ProductID}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region DELETE_ROUTES
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await repo.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting Product by Id.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

    }
}
