/**
 * @file    AdministratorRepoADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an AdministratorRepo for ADO that implements
 *              CRUD services for the Administrators Table, to be used
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
    public class AdministratorRepoADO(
        ILogger<AdministratorRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<AdministratorEntityADO, int>
    {
        #region CREATE_ROUTES
        public async Task<bool> InsertAsync(AdministratorEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                AdministratorDTO dto = AdministratorMapper.ToDto(entity);
                var parameters = new List<SqlParameterModel>
                {
                    new("@EmailAddress", System.Data.SqlDbType.VarChar, dto.EmailAddress!),
                    new("@Password", System.Data.SqlDbType.VarChar, dto.Password!),
                    new("@FirstName", System.Data.SqlDbType.VarChar, dto.FirstName!),
                    new("@LastName", System.Data.SqlDbType.VarChar, dto.LastName!)
                };

                const string cmd = @"INSERT INTO Administrators
                                    (
                                        EmailAddress,
                                        Password,
                                        FirstName,
                                        LastName
                                    )
                                    VALUES
                                    (
                                        @EmailAddress,
                                        @Password,
                                        @FirstName,
                                        @LastName
                                    )";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters) != 0 ? true : false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting the Administrator!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion CREATE_ROUTES

        #region READ_ROUTES
        public async Task<IEnumerable<AdministratorEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Administrators");

                var admins = new List<AdministratorEntityADO>();

                while (await  reader.ReadAsync())
                {
                    admins.Add(GetAdminFromReader(reader) ?? throw new Exception("Error Selecting Administrators"));
                }

                return admins;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Administrator list");
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<AdministratorEntityADO?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@AdminID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Administrators WHERE AdminID = @AdminID";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var admin = reader != null ? await GetSingleAdminFromReader(reader) : null;
                return admin;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving admin with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //TODO: Add FindByUnique!
        #endregion READ_ROUTES

        #region UPDATE_ROUTES
        public async Task<bool> UpdateAsync(int id, AdministratorEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                AdministratorDTO dto = AdministratorMapper.ToDto(entity);
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@AdminID", System.Data.SqlDbType.Int, id),
                    new("@EmailAddress", System.Data.SqlDbType.VarChar, dto.EmailAddress!),
                    new("@Password", System.Data.SqlDbType.VarChar, dto.Password!),
                    new("@FirstName", System.Data.SqlDbType.VarChar, dto.FirstName!),
                    new("@LastName", System.Data.SqlDbType.VarChar, dto.LastName!)
                };

                const string cmd = @"UPDATE Administrators
                                        SET EmailAddress = @EmailAddress, 
                                            Password = @Password,
                                            FirstName = @FirstName,
                                            LastName = @LastName
                                        WHERE AdminID = @AdminID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters) != 0 ? true : false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating the Adminstrator!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region DELETE_ROUTES
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@AdminID", System.Data.SqlDbType.Int, id)
                };

                var cmd = @"DELETE Administrators 
                                WHERE AdminID = @AdminID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters) != 0 ? true : false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the administrator");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region HELPERS
        private async Task<AdministratorEntityADO?> GetSingleAdminFromReader(SqlDataReader reader)
        {
            try
            {
                await reader.ReadAsync();

                return GetAdminFromReader(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private AdministratorEntityADO? GetAdminFromReader(SqlDataReader reader)
        {
            try
            {
                var admin = new AdministratorEntityADO()
                {
                    AdminId = reader.GetInt32(reader.GetOrdinal("AdminID")),
                    EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                };

                return admin;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
