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
        public async Task<IEnumerable<CustomerEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Customers");

                var customers = new List<CustomerEntityADO>();

                while (await reader.ReadAsync())
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
                        ShippingAddressID = reader.IsDBNull(shippingOrd) ?  null : reader.GetInt32(shippingOrd),
                        BillingAddressID = reader.IsDBNull(billingOrd) ? null : reader.GetInt32(billingOrd)
                    };
                    customers.Add(customer);
                }

                return customers;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Customers list");
                throw new Exception(ex.Message, ex);
            }
        }
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
        private async Task<CustomerEntityADO> GetSingleCustomerFromReader(SqlDataReader reader)
        {
            try
            {
                await reader.ReadAsync();

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
