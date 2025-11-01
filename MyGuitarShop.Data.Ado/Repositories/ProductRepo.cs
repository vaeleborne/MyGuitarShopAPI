using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
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
    :  IRepository<ProductEntity> 
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
                logger.LogError(ex.Message, $"Error retrieving product with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }

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
                throw new Exception(ex.Message, ex);
            }
        }

        public Task<int> InsertAsync(ProductDTO dto)
        {
            throw new NotImplementedException();

            //DateTime.UtcNow
        }

        public async Task<int> UpdateAsync(ProductDTO dto)
        {
            try
            {
                //Setting up the command and parameters
                var parameters = new List<SqlParameterModel>
                {
                    new("@ProductID", System.Data.SqlDbType.Int, dto.ProductID!),
                    new("@ProductName",  System.Data.SqlDbType.Text, dto.ProductName!),
                    new("@ListPrice", System.Data.SqlDbType.Money, dto.ListPrice!),
                    new("@DiscountPercent", System.Data.SqlDbType.Decimal, dto.DiscountPercent!)
                };

                const string cmd = @"UPDATE Products
                                        SET ProductName = @ProductName, ListPrice = @ListPrice, DiscountPercent = @DiscountPercent
                                        WHERE ProductID = @ProductID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex.Message, "Error updating the product");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
