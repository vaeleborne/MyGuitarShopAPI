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
    public class CategoryRepoADO(
        ILogger<CategoryRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<CategoryEntityADO>
    {
        #region CREATE_ROUTES
        public async Task<int> InsertAsync(CategoryEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                CategoryDTO dto = CategoryMapper.ToDto(entity);
                var parameters = new List<SqlParameterModel>
                {
                    new("@CategoryName", System.Data.SqlDbType.VarChar, dto.CategoryName!)
                };

                const string cmd = @"INSERT INTO Categories
                                    (
                                        CategoryName
                                    )
                                    VALUES
                                    (
                                        @CategoryName
                                    )";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting the Category!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region READ_ROUTES
        public async Task<IEnumerable<CategoryEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Categories");

                var categories = new List<CategoryEntityADO>();

                while (await reader.ReadAsync())
                {
                    categories.Add(GetCategoryFromReader(reader) ?? throw new Exception("Error Selecting Categories"));
                }

                return categories;
            }
            catch ( Exception ex )
            {
                logger.LogError(ex.Message, "Error retrieving Categories list");
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<CategoryEntityADO?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@CategoryID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Categories WHERE CategoryID = @CategoryID";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var admin = reader != null ? await GetSingleCategoryFromReader(reader) : null;
                return admin;
            }
            catch ( Exception ex )
            {
                logger.LogError(ex.Message, $"Error retrieving category with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<CategoryEntityADO?> FindByUniqueAsync(string categoryName)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@CategoryName", System.Data.SqlDbType.VarChar, categoryName)
                };

                var cmd = $"SELECT * FROM Categories WHERE CategoryName = @CategoryName";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var product = reader != null ? await GetSingleCategoryFromReader(reader) : null;
                return product;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving product with product name: {categoryName}", ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region UPDATE_ROUTES
        public async Task<int> UpdateAsync(int id, CategoryEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                CategoryDTO dto = CategoryMapper.ToDto(entity);
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@CategoryID", System.Data.SqlDbType.Int, id),
                    new("@CategoryName", System.Data.SqlDbType.VarChar, dto.CategoryName!)
                };

                const string cmd = @"UPDATE Categories
                                        SET CategoryName = @CategoryName
                                        WHERE CategoryID = @CategoryID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating the Category!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region DELETE_ROUTES
        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@CategoryID", System.Data.SqlDbType.Int, id)
                };

                var cmd = @"DELETE Categories 
                                WHERE CategoryID = @CategoryID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the Category");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region HELPERS
        private async Task<CategoryEntityADO?> GetSingleCategoryFromReader(SqlDataReader reader)
        {
            try
            {
                await reader.ReadAsync();
                return GetCategoryFromReader(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private CategoryEntityADO? GetCategoryFromReader(SqlDataReader reader)
        {
            try
            {
                var category = new CategoryEntityADO()
                {
                    CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"))
                };

                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion HELPERS
    }
}
