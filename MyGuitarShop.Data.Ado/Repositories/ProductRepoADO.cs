/**
 * @file    ProductRepoADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a ProductRepo for ADO that implements
 *              CRUD services for the Products Table, to be used
 *              by a Controller specific to ADO in the API Project.
 * @date    2025-11-11
 * @version 1.0
 */
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repositories.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    /// <summary>
    /// Implementation of the Product Repo containing CRUD services.
    /// </summary>
    /// <param name="logger">Logger to use</param>
    /// <param name="connectionFactory">Factory to use for SQL connections</param>
    public class ProductRepoADO
    (
        ILogger<ProductRepoADO> logger,
        SqlConnectionFactory connectionFactory
    ) 
    :  IRepository<ProductEntityADO> 
    {
        #region CREATION_SERVICES
        /// <summary>
        /// Implementation of the CREATE service to insert a new Product into the DB.
        /// </summary>
        /// <param name="entity">The product entity to insert</param>
        /// <returns>The number of items inserted (success would be 1)</returns>
        /// <exception cref="Exception">Logs exception then throws to caller.</exception>
        public async Task<int> InsertAsync(ProductEntityADO entity)
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

        #region READ_SERVICES
        /// <summary>
        /// Implementation of a READ service to get a Product from its ID.
        /// </summary>
        /// <param name="id">The id of the product to find.</param>
        /// <returns>The Product Entity Corresponding to {id}, or null if no such product is found.</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<ProductEntityADO?> FindByIdAsync(int id)
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
                var product = reader != null ? await GetSingleProductFromReader(reader) : null;
                return product;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving product with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Implementation of a READ service to get a Product given its unique product name.
        /// </summary>
        /// <param name="productName">The name of the product to find.</param>
        /// <returns>The Product Entity Corresponding to its {productName}, or null if no such product is found.</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<ProductEntityADO?> FindByUniqueAsync(string productName)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@ProductName", System.Data.SqlDbType.VarChar, productName)
                };

                var cmd = $"SELECT * FROM Products WHERE ProductName = @ProductName";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var product = reader != null ? await GetSingleProductFromReader(reader) : null;
                return product;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving product with product name: {productName}", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        ///  Implementation of a READ service to get all Products in the DB.
        /// </summary>
        /// <returns>A list of all Product Entities</returns>
        /// <exception cref="Exception">Will log then throw to caller</exception>
        public async Task<IEnumerable<ProductEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Products");

                //Reader Parsing
                var products = reader != null ? await GetProductsFromReader(reader) : null;
                return products ?? new List<ProductEntityADO>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving product list");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region UPDATE_SERVICES
        /// <summary>
        /// Implementation of an UPDATE service to update a Product given it's id, and every property.
        /// </summary>
        /// <param name="id">Id of the Product To update.</param>
        /// <param name="entity">Product Entity representing what the changes should be.</param>
        /// <returns>Number of items updates (success should be 1)</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<int> UpdateAsync(int id, ProductEntityADO entity)
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

        #region DELETE_SERVICES
        /// <summary>
        /// Implements a DELETE service to delete a Product from the DB.
        /// </summary>
        /// <param name="id">Id of the Product to delete.</param>
        /// <returns>Number of items deleted (success should be 1).</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
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

        #region HELPERS
        /// <summary>
        /// Helper to retrieve a single ProductEntity (or null) given a reader from a query.
        /// </summary>
        /// <param name="reader">The reader to use</param>
        /// <returns>A ProductEntity from the Reader, or null if one could not be found</returns>
        private static async Task<ProductEntityADO?> GetSingleProductFromReader(SqlDataReader reader)
        {
            //Attempt to create a ProductEntity from the reader
            try
            {
                await reader.ReadAsync();
                var product = new ProductEntityADO
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                    DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                    DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                };

                return product;
            }
            catch (Exception ex)
            {
                //On error, assume not found, return null.
                return null;
            }
        }

        /// <summary>
        /// Helper to get all product entities within a given reader
        /// </summary>
        /// <param name="reader">The reader to use</param>
        /// <returns>A list of Product Entities that can be found from the reader</returns>
        /// <exception cref="Exception">Throws error if none can be found, or if there is an issue with any.</exception>
        private static async Task<List<ProductEntityADO>> GetProductsFromReader(SqlDataReader reader)
        {
            //List to contain the entities
            var products = new List<ProductEntityADO>();

            //Parse the reader, adding to the products variable as we go.
            try
            {
                while (await reader.ReadAsync())
                {
                    var product = new ProductEntityADO
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                        ProductCode = reader.GetString(reader.GetOrdinal("ProductCode")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                        DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                        DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                    };
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                //On error, throw error to caller for caller to handle.
                throw new Exception(ex.Message, ex);
            }

            return products;
        }
        #endregion

    }
}
