using GuitarShopAPI.Mappers;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Common.Interfaces;

namespace GuitarShopAPI.Abstract
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public abstract class BaseController<TDto, TEntity>(
        IRepository<TEntity, int> repo,
        ILogger<BaseController<TDto, TEntity>> logger
        ) : ControllerBase where TEntity : class, new()
    {

        #region CREATE_ROUTES
        [HttpPost]
        public virtual async Task<IActionResult> CreateAsync(TDto dto)
        {
            try
            {
                var entity = AutoReflectionMapper.Map<TDto, TEntity>(dto)
                     ?? throw new InvalidOperationException("Unable to map Dto to Entity.");

                var entitiesCreated = await repo.InsertAsync(entity);

                return Ok($"{entitiesCreated} entities created.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding a new entity.");
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
                var entities = await repo.GetAllAsync();

                return entities.Any() ? Ok(entities) : NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching entities");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var entity = await repo.FindByIdAsync(id);

                if (entity == null)
                    return NotFound($"Entity with id {id} not found");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching entity.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region UPDATE_ROUTES
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, TDto dto)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Entity with id {id} not found.");

                var entity = AutoReflectionMapper.Map<TDto, TEntity>(dto)
                     ?? throw new InvalidOperationException("Unable to map Dto to Entity.");

                var numberUpdated = await repo.UpdateAsync(id, entity);

                return Ok($"{numberUpdated} entities updated.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating entity with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region DELETE_ROUTES
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                if (await repo.FindByIdAsync(id) == null)
                    return NotFound($"Entity with id {id} not found.");

                var numberDeleted = await repo.DeleteAsync(id);

                return Ok($"{numberDeleted} enitites deleted.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting Entity.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

    }
}
