/**
 * @file    AddressRepoADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an AddressRepo for ADO that implements
 *              CRUD services for the Addresses Table, to be used
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
    /// Implementation of the Address Repo containing CRUD services.
    /// </summary>
    /// <param name="logger">Logger to use</param>
    /// <param name="connectionFactory">Factory to use for SQL connections</param>
    public class AddressRepoADO(
        ILogger<AddressRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<AddressEntityADO>

    {

        #region CREATE_ROUTES
        //TODO: Update to be a transaction
        public async Task<int> InsertAsync(AddressEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                AddressesDTO dto = AddressMapper.ToDto(entity);
                var parameters = new List<SqlParameterModel>
                {
                    new("@CustomerID", System.Data.SqlDbType.Int, (object?)dto.CustomerId ?? DBNull.Value),
                    new("@Line1", System.Data.SqlDbType.VarChar, dto.Line1!),
                    new("@Line2", System.Data.SqlDbType.VarChar, (object?)dto.Line2 ?? DBNull.Value),
                    new("@City", System.Data.SqlDbType.VarChar, dto.City!),
                    new("@State", System.Data.SqlDbType.VarChar, dto.State!),
                    new("@ZipCode", System.Data.SqlDbType.VarChar, dto.ZipCode!),
                    new("@Phone", System.Data.SqlDbType.VarChar, dto.Phone!),
                    new("@Disabled", System.Data.SqlDbType.Int, dto.Disabled!)
                };

                const string cmd = @"INSERT INTO Addresses
                                    (
                                        CustomerID,
                                        Line1,
                                        Line2,
                                        City,
                                        State,
                                        ZipCode,
                                        Phone,
                                        Disabled
                                    )
                                    VALUES
                                    (
                                        @CustomerID,
                                        @Line1,
                                        @Line2,
                                        @City,
                                        @State,
                                        @ZipCode,
                                        @Phone,
                                        @Disabled
                                    )";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting the Address!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region READ_ROUTES
        public async Task<IEnumerable<AddressEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Addresses");

                var addresses = new List<AddressEntityADO>();

                while (await reader.ReadAsync())
                {
                    addresses.Add(GetAddressFromReader(reader) ?? throw new Exception("Error Selecting Addresses"));
                }

                return addresses;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Addresses list");
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<AddressEntityADO?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@AddressID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Addresses WHERE AddressID = @AddressID";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                await reader.ReadAsync();

                return GetAddressFromReader(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving category with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region UPDATE_ROUTES
        public async Task<int> UpdateAsync(int id, AddressEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                AddressesDTO dto = AddressMapper.ToDto(entity);

                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@AddressID", System.Data.SqlDbType.Int, id),
                    new("@CustomerID", System.Data.SqlDbType.Int, (object?)dto.CustomerId ?? DBNull.Value),
                    new("@Line1", System.Data.SqlDbType.VarChar, dto.Line1!),
                    new("@Line2", System.Data.SqlDbType.VarChar, (object?)dto.Line2 ?? DBNull.Value),
                    new("@City", System.Data.SqlDbType.VarChar, dto.City!),
                    new("@State", System.Data.SqlDbType.VarChar, dto.State!),
                    new("@ZipCode", System.Data.SqlDbType.VarChar, dto.ZipCode!),
                    new("@Phone", System.Data.SqlDbType.VarChar, dto.Phone!),
                    new("@Disabled", System.Data.SqlDbType.Int, dto.Disabled!)
                };

                const string cmd = @"UPDATE Addresses
                                        SET CustomerID = @CustomerID,
                                            Line1 = @Line1,
                                            Line2 = @Line2,
                                            City = @City,
                                            State = @State,
                                            ZipCode = @ZipCode,
                                            Phone = @Phone,
                                            Disabled = @Disabled
                                        WHERE AddressID = @AddressID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating the Address!");
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
                    new("@AddressID", System.Data.SqlDbType.Int, id)
                };

                var cmd = @"DELETE Addresses 
                                WHERE AddressID = @AddressID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the Address");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region HELPERS
        private AddressEntityADO? GetAddressFromReader(SqlDataReader reader)
        {
            try
            {
                int custOrd = reader.GetOrdinal("CustomerID");
                int line2Ord = reader.GetOrdinal("Line2");

                var address = new AddressEntityADO()
                {
                    AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                    CustomerID = reader.IsDBNull(custOrd) ? null : reader.GetInt32(custOrd),
                    Line1 = reader.GetString(reader.GetOrdinal("Line1")),
                    Line2 = reader.IsDBNull(line2Ord) ? null : reader.GetString(line2Ord),
                    City = reader.GetString(reader.GetOrdinal("City")),
                    State = reader.GetString(reader.GetOrdinal("State")),
                    ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                    Disabled = reader.GetInt32(reader.GetOrdinal("Disabled"))
                };

                return address;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
