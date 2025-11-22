using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.MongoDb.Models;
using MyGuitarShop.Data.MongoDb.Services;
using System.Formats.Asn1;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace GuitarShopAPI.Controllers.MongoControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsMongoController (
        ILogger<ProductsMongoController> logger,
        MongoProductService productService)
        : ControllerBase
    {
        [HttpGet]
        public async Task <IActionResult> GetAsync()
        {
            try
            {
                var products = await productService.GetAllAsync();

                if (products.Count() != 0)
                    return Ok(products);

                return NotFound("No items found");
            } catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetByIdAsync(string id)
        {
            try
            {
                var product = await productService.FindByIdAsync(id);

                return product != null ? Ok(product) : NotFound();
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving product with ID {ProductID}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task <IActionResult> CreateProductAsync(ProductDTO new_product)
        {
            try
            {
                var model = new ProductModel()
                {
                    ProductCode = new_product.ProductCode,
                    ProductName = new_product.ProductName,
                    Description = new_product.Description,
                    ListPrice = new_product.ListPrice ?? 0,
                    DiscountPercent = new_product.DiscountPercent ?? 0
                };

                if (await productService.InsertAsync(model))
                    return Ok("Product Inserted");

                throw new Exception("Unable to insert new product");
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding new product. \n\nError {message}", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut("id")]
        public async Task <IActionResult> UpdateProductAsync(string id, ProductDTO updated_product)
        {
            try
            {
                if (await productService.FindByIdAsync(id) == null)
                    return NotFound($"ProductID {id} not found");

                var entity = new ProductModel()
                {
                    _id = id,
                    ProductCode = updated_product.ProductCode,
                    ProductName = updated_product.ProductName,
                    Description = updated_product.Description,
                    ListPrice = updated_product.ListPrice ?? 0,
                    DiscountPercent = updated_product.DiscountPercent ?? 0
                };

                var number_updated = await productService.UpdateAsync(id, entity);

                return Ok($"{number_updated} products updated.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating product with ID {ProductID}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpDelete]
        public async Task <IActionResult> DeleteProductAsync(string id)
        {
            try
            {
                if (await productService.FindByIdAsync(id) == null)
                    return NotFound($"ProductID {id} not found");

                if (await productService.DeleteAsync(id))
                    return Ok($"ProductID {id} deleted");

                throw new Exception($"Unable to delete ProductID {id}");
            }
            catch (Exception ex)
            {
                logger.LogError("Error deleting ProductID {ProductID}\n\nError {message}", id, ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
