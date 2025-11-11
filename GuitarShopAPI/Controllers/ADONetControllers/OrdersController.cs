using GuitarShopAPI.Abstract;
using GuitarShopAPI.Mappers;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Repositories;

namespace GuitarShopAPI.Controllers.ADONetControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersADONetController(
        ILogger<OrdersADONetController> logger,
        OrderRepoADO repo)
        : BaseController<OrderDTO, OrderEntityADO>(repo, logger)
    {
        [HttpPost]
        public override async Task<IActionResult> CreateAsync(OrderDTO dto)
        {
            try
            {
                var entity = AutoReflectionMapper.Map<OrderDTO, OrderEntityADO>(dto)
                     ?? throw new InvalidOperationException("Unable to map Dto to Entity.");

                var createdID = await repo.InsertAsync(entity);

                return Ok($"Created New Order With ID{createdID}!");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding a new entity.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
