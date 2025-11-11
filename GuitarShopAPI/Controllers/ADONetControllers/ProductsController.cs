using GuitarShopAPI.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace GuitarShopAPI.Controllers.ADONetControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsADONetController(
        ILogger<ProductsADONetController> logger,
        ProductRepoADO repo) 
        : BaseController<ProductDTO, ProductEntityADO>(repo, logger)
    {
        [HttpGet("search")]
        public async Task<IActionResult> FindByUniqueAsync([FromQuery] string productName )
        {
            if (string.IsNullOrEmpty(productName))
            {
                return BadRequest("Product Name Cannot Be Empty.");
            }
            try
            {
                
                var entity = await repo.FindByUniqueAsync(productName);

                if (entity == null)
                    return NotFound($"Product with name {productName} not found");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching product.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
