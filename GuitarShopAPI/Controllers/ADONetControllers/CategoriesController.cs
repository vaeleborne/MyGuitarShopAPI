using GuitarShopAPI.Abstract;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;

namespace GuitarShopAPI.Controllers.ADONetControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesADONetController(
        ILogger<CategoriesADONetController> logger,
        CategoryRepoADO repo)
        : BaseController<CategoryDTO, CategoryEntityADO>(repo, logger)
    {
        [HttpGet("search")]
        public async Task<IActionResult> FindByUniqueAsync([FromQuery] string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Category Name Cannot Be Empty.");
            }
            try
            {
                var entity = await repo.FindByUniqueAsync(categoryName);

                if (entity == null)
                    return NotFound($"Category with name {categoryName} not found");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Category.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
