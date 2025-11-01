using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class ProductRepo
    (
        ILogger<ProductRepo> logger,
        SqlConnectionFactory connectionFactory
    ) 
    : IRepository<ProductEntity> 
    {
        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductEntity?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@ProductID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Products WHERE productID = {id}";
                using SqlDataReader? reader =   await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory, 
                                                $"SELECT * FROM Products WHERE productID = @ProductID",
                                                parameters);
               
                //Reader Parsing
                var product = reader != null ? await RepoHelpers.GetSingleProductFromReader(reader) : null;
                return product;
            }
            catch (Exception ex )
            {
                logger.LogError(ex.Message, $"Error retrieving product with id: {id}");
            }

            return null;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Products");

                //Reader Parsing
                var products = reader != null ? await RepoHelpers.GetProductsFromReader(reader) : null;
                return products ?? new List<ProductEntity>();
            } 
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product list");
            }
            return new List<ProductEntity>(); 
        }

        public Task<int> InsertAsync(ProductEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(ProductEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
