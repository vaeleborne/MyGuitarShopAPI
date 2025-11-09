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
        #region CREATION_TASKS
        public async Task<int> InsertAsync(ProductEntity entity)
        {
            try
            {
                //Setting up the command and parameters
                ProductDTO dto = ProductMapper.ToDto(entity);
                var parameters = new List<SqlParameterModel>
                {
                    new("@CategoryID", System.Data.SqlDbType.Int, dto.CategoryID!),
                    new("@ProductCode", System.Data.SqlDbType.Text, dto.ProductCode!),
                    new("@ProductName",  System.Data.SqlDbType.Text, dto.ProductName!),
                    new("@Description", System.Data.SqlDbType.Text, dto.Description!),
                    new("@ListPrice", System.Data.SqlDbType.Money, dto.ListPrice!),
                    new("@DiscountPercent", System.Data.SqlDbType.Decimal, dto.DiscountPercent!),
                    new("@DateAdded", System.Data.SqlDbType.DateTime, dto.DateAdded!)
                };

                const string cmd = @"INSERT INTO Products 
                                        (
                                            CategoryID, 
                                            ProductCode, 
                                            ProductName, 
                                            Description,
                                            ListPrice, 
                                            DiscountPercent,
                                            DateAdded
                                        )
                                        VALUES 
                                        (
                                            @CategoryID,
                                            @ProductCode,
                                            @ProductName,
                                            @Description,
                                            @ListPrice,
                                            @DiscountPercent,
                                            @DateAdded
                                        )";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting the product");
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region READ_TASKS
        public async Task<ProductEntity?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@ProductID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Products WHERE productID = @ProductID";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var product = reader != null ? await RepoHelpers.GetSingleProductFromReader(reader) : null;
                return product;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving product with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }

        }
        public async Task<ProductEntity?> FindByUniqueAsync(string productName)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@ProductName", System.Data.SqlDbType.VarChar, productName)
                };

                var cmd = $"SELECT * FROM Products WHERE productName = @ProductName";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var product = reader != null ? await RepoHelpers.GetSingleProductFromReader(reader) : null;
                return product;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving product with product name: {productName}", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets all Products from Products in the DB
        /// </summary>
        /// <returns>A list of Products, or null </returns>
        /// <exception cref="Exception">Will log then throw again</exception>
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
        #endregion

        #region UPDATE_TASKS
        public async Task<int> UpdateAsync(int id, ProductEntity entity)
        {
            try
            {
                //Setting up the command and parameters
                ProductDTO dto = ProductMapper.ToDto(entity);
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
        #endregion

        #region DELETE_TASKS
        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                //Setting up the command and parameters
                var parameters = new List<SqlParameterModel>
                {
                    new("@ProductID", System.Data.SqlDbType.Int, id)
                };

                const string cmd = @"DELETE Products
                                        WHERE ProductID = @ProductID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the product");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

    }
}
