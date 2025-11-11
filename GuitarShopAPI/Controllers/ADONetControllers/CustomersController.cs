using GuitarShopAPI.Abstract;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;

namespace GuitarShopAPI.Controllers.ADONetControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersADONetController(
        ILogger<CustomersADONetController> logger,
        CustomerRepoADO repo)
        : BaseController<CustomerDTO, CustomerEntityADO>(repo, logger)
    {
        [HttpGet("search")]
        public async Task<IActionResult> FindByUniqueAsync([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email Address Cannot Be Empty.");
            }
            try
            {
                var entity = await repo.FindByUniqueAsync(email);

                if (entity == null)
                    return NotFound($"Customer with email address {email} not found");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching Customer.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
