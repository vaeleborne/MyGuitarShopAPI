using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.EFCore.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GuitarShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(
        ILogger<HealthController> logger, 
        SqlConnectionFactory sqlConnectionFactory,
        MyGuitarShopContext dbContext)
        : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Healthy");
            }
            catch (Exception ex)
            {
                logger.LogWarning("Health check failed unreasonably.");
                return StatusCode(503, "Unhealthy");
            }
        }

        [HttpGet("db/ado")]
        public IActionResult GetDbHealth()
        {
            try
            {
                using var connection = sqlConnectionFactory.OpenSqlConnection();

                return Ok(new { Message = "Connection successful", connection.Database });
            }
            catch (Exception ex)
            {
                logger.LogCritical("Database health check failed.");

                return StatusCode(503, "Database unhealthy");

            }
        }

        [HttpGet("db/efcore")]
        public async Task<IActionResult> GetDbContextHealthAsync()
        {
            try
            {
                if (!await dbContext.Database.CanConnectAsync())
                    throw new Exception("Cannot connect to database via EF Core DbContext");

                return Ok(new { Message = "Connection successful", dbContext.Database });
            }
            catch
            {
                logger.LogCritical("Database health check failed.");
                return StatusCode(503, "Database unhealthy.");
            }
        }
    }
}
