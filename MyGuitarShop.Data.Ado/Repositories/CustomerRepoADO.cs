/**
 * @file    CustomerRepoADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a CustomerRepo for ADO that implements
 *              CRUD services for the Customers Table, to be used
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
    public class CustomerRepoADO(
        ILogger<CustomerRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<CustomerEntityADO>
    {
        #region CREATE_ROUTES
        /// <summary>
        /// Implementation of the CREATE service to insert a new Customer into the DB.
        /// </summary>
        /// <param name="entity">The Customer entity to insert</param>
        /// <returns>The number of items inserted (success would be 1)</returns>
        /// <exception cref="Exception">Logs exception then throws to caller.</exception>
        public async Task<int> InsertAsync(CustomerEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                CustomerDTO dto = CustomerMapper.ToDto(entity);
                var parameters = new List<SqlParameterModel>
                {
                    new("@EmailAddress", System.Data.SqlDbType.VarChar, dto.EmailAddress!),
                    new("@Password", System.Data.SqlDbType.VarChar, dto.Password!),
                    new("@FirstName", System.Data.SqlDbType.VarChar, dto.FirstName!),
                    new("@LastName", System.Data.SqlDbType.VarChar, dto.LastName!),
                    new("@ShippingAddressID", System.Data.SqlDbType.Int, (object?)dto.ShippingAddressId ?? DBNull.Value),
                    new("@BillingAddressID", System.Data.SqlDbType.Int, (object?)dto.BillingAddressId ?? DBNull.Value)
                };

                const string cmd = @"INSERT INTO Customers
                                    (
                                        EmailAddress,
                                        Password,
                                        FirstName,
                                        LastName,
                                        ShippingAddressID,
                                        BillingAddressID
                                    )
                                    VALUES
                                    (
                                        @EmailAddress,
                                        @Password,
                                        @FirstName,
                                        @LastName,
                                        @ShippingAddressID,
                                        @BillingAddressID
                                    )";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting the Customer!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region READ_ROUTES
        /// <summary>
        ///  Implementation of a READ service to get all Customers in the DB.
        /// </summary>
        /// <returns>A list of all Customer Entities</returns>
        /// <exception cref="Exception">Will log then throw to caller</exception>
        public async Task<IEnumerable<CustomerEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Customers");

                var customers = new List<CustomerEntityADO>();

                while (await reader.ReadAsync())
                {
                    customers.Add(GetCustomerFromReader(reader) ?? throw new Exception("Error Selecting Customers"));
                }

                return customers;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Customers list");
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Implementation of a READ service to get a Customer from the CustomerID.
        /// </summary>
        /// <param name="id">The id of the Customer to find.</param>
        /// <returns>The Customer Entity Corresponding to {id}, or null if no such customer is found.</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<CustomerEntityADO?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@CustomerID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var admin = reader != null ? await GetSingleCustomerFromReader(reader) : null;
                return admin;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving admin with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Implementation of a READ service to get a Customer given their Email Address.
        /// </summary>
        /// <param name="email">The email to lookup.</param>
        /// <returns>The Product Entity Corresponding to its {email}, or null if no such product is found.</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<CustomerEntityADO?> FindByUniqueAsync(string email)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@EmailAddress", System.Data.SqlDbType.VarChar, email)
                };

                var cmd = $"SELECT * FROM Customers WHERE EmailAddress = @EmailAddress";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                var product = reader != null ? await GetSingleCustomerFromReader(reader) : null;
                return product;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving customer with email name: {email}", ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region UPDATE_ROUTES
        public async Task<int> UpdateAsync(int id, CustomerEntityADO entity)
        {
            try
            {
                //Setting up the command and parameters
                CustomerDTO dto = CustomerMapper.ToDto(entity);
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@CustomerID", System.Data.SqlDbType.Int, id),
                    new("@EmailAddress", System.Data.SqlDbType.VarChar, dto.EmailAddress!),
                    new("@Password", System.Data.SqlDbType.VarChar, dto.Password!),
                    new("@FirstName", System.Data.SqlDbType.VarChar, dto.FirstName!),
                    new("@LastName", System.Data.SqlDbType.VarChar, dto.LastName!),
                    new("@ShippingAddressID", System.Data.SqlDbType.Int, (object?)dto.ShippingAddressId ?? DBNull.Value),
                    new("@BillingAddressID", System.Data.SqlDbType.Int, (object?)dto.BillingAddressId ?? DBNull.Value)
                };

                const string cmd = @"UPDATE Customers
                                        SET EmailAddress = @EmailAddress, 
                                            Password = @Password,
                                            FirstName = @FirstName,
                                            LastName = @LastName,
                                            ShippingAddressID = @ShippingAddressID,
                                            BillingAddressID = @BillingAddressID
                                        WHERE CustomerID = @CustomerID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating the Adminstrator!");
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
                    new("@CustomerID", System.Data.SqlDbType.Int, id)
                };

                var cmd = @"DELETE Customers 
                                WHERE CustomerID = @CustomerID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the customer");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region HELPERS
        private async Task<CustomerEntityADO?> GetSingleCustomerFromReader(SqlDataReader reader)
        {
            try
            {
                await reader.ReadAsync();
                return GetCustomerFromReader(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private CustomerEntityADO? GetCustomerFromReader(SqlDataReader reader)
        {
            try
            {
                int shippingOrd = reader.GetOrdinal("ShippingAddressID");
                int billingOrd = reader.GetOrdinal("BillingAddressID");
                var customer = new CustomerEntityADO()
                {
                    CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                    EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress")),
                    Password = reader.GetString(reader.GetOrdinal("Password")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    ShippingAddressID = reader.IsDBNull(shippingOrd) ? null : reader.GetInt32(shippingOrd),
                    BillingAddressID = reader.IsDBNull(billingOrd) ? null : reader.GetInt32(billingOrd)
                };

                return customer;
            }
            catch(Exception ex) 
            {
                return null;
            }
        }
        #endregion
    }
}
