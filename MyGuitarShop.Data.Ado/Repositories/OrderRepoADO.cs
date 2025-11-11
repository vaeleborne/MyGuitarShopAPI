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
    public class OrderRepoADO(
        ILogger<OrderRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<OrderEntityADO>
    {
        #region CREATE_ROUTES
        public async Task<int> InsertAsync(OrderEntityADO entity)
        {      
            try
            {
                //Setup Connection For Transaction
                using var conn = await connectionFactory.OpenSqlConnectionAsync();
                using var transaction = conn.BeginTransaction();

                try
                {
                    object P(object? v) => v ?? DBNull.Value;
                    //Insert Order first
                    using var orderCmd = conn.CreateCommand();

                    orderCmd.Transaction = transaction;

                    OrderDTO dto = OrderMapper.ToDto(entity);
                    var parameters = new List<SqlParameterModel>
                    {
                        new("@CustomerID", System.Data.SqlDbType.Int, P(dto.CustomerId)),
                        new("@OrderDate", System.Data.SqlDbType.DateTime, dto.OrderDate),
                        new("@ShipAmount", System.Data.SqlDbType.Money, dto.ShipAmount),
                        new("@TaxAmount", System.Data.SqlDbType.Money,dto.TaxAmount),
                        new("@ShipDate", System.Data.SqlDbType.DateTime, P(dto.ShipDate)),
                        new("@ShipAddressID", System.Data.SqlDbType.Int, dto.ShipAddressId),
                        new("@CardType", System.Data.SqlDbType.VarChar, dto.CardType),
                        new("@CardNumber", System.Data.SqlDbType.VarChar, dto.CardNumber),
                        new("@CardExpires", System.Data.SqlDbType.VarChar, dto.CardExpires),
                        new("@BillingAddressID", System.Data.SqlDbType.Int, dto.BillingAddressId)

                    };

                    orderCmd.CommandText = @"INSERT INTO Orders
                                                (
                                                    CustomerID,
                                                    OrderDate,
                                                    ShipAmount,
                                                    TaxAmount,
                                                    ShipDate,
                                                    ShipAddressID,
                                                    CardType,
                                                    CardNumber,
                                                    CardExpires,
                                                    BillingAddressID
                                                )
                                                OUTPUT INSERTED.OrderID
                                                VALUES
                                                (
                                                    @CustomerID,
                                                    @OrderDate,
                                                    @ShipAmount,
                                                    @TaxAmount,
                                                    @ShipDate,
                                                    @ShipAddressID,
                                                    @CardType,
                                                    @CardNumber,
                                                    @CardExpires,
                                                    @BillingAddressID 
                                                );";

                    AddParameters(orderCmd, parameters);

                    var newOrderIDObject = await orderCmd.ExecuteScalarAsync();
                    if (newOrderIDObject == null)
                        throw new Exception("Failed to retrieve new OrderID from insert!");

                    int newOrderId = Convert.ToInt32(newOrderIDObject);

                    //Insert OrderItems
                    if (entity.Items != null && entity.Items.Any())
                    {
                        using var itemCmd = conn.CreateCommand();

                        itemCmd.CommandText = @"INSERT INTO OrderItems
                                                (
                                                   OrderID,
                                                   ProductID,
                                                   ItemPrice,
                                                   DiscountAmount,
                                                   Quantity
                                                )
                                                OUTPUT INSERTED.ItemID
                                                VALUES
                                                (
                                                   @OrderID,
                                                   @ProductID,
                                                   @ItemPrice,
                                                   @DiscountAmount,
                                                   @Quantity
                                                );";

                        itemCmd.Parameters.Add(new SqlParameter("@OrderID", System.Data.SqlDbType.Int));
                        itemCmd.Parameters.Add(new SqlParameter("@ProductID", System.Data.SqlDbType.Int));
                        itemCmd.Parameters.Add(new SqlParameter("@ItemPrice", System.Data.SqlDbType.Money));
                        itemCmd.Parameters.Add(new SqlParameter("@DiscountAmount", System.Data.SqlDbType.Money));
                        itemCmd.Parameters.Add(new SqlParameter("@Quantity", System.Data.SqlDbType.Int));

                        foreach (var item in entity.Items )
                        {
                            itemCmd.Parameters["@OrderID"].Value = newOrderId;
                            itemCmd.Parameters["@ProductID"].Value = item.ProductID;
                            itemCmd.Parameters["@ItemPrice"].Value = item.ItemPrice;
                            itemCmd.Parameters["@DiscountAmount"].Value = item.DiscountAmount;
                            itemCmd.Parameters["@Quantity"].Value = item.Quantity;

                            await itemCmd.ExecuteScalarAsync();
                        }  
                    }

                    //Success, commit
                    transaction.Commit();
                    return newOrderId;
                }
                catch (Exception transactEx)
                {
                    transaction.Rollback();
                    throw new Exception(transactEx.Message, transactEx);
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error inserting Order!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region READ_ROUTES
        public async Task<IEnumerable<OrderEntityADO>> GetAllAsync()
        {
            try
            {
                //Connection Setup & Execution
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(connectionFactory, "SELECT * FROM Orders");

                var orders = new List<OrderEntityADO>();

                while (await reader.ReadAsync())
                {
                    var order = GetOrderFromReader(reader);
                    if (order == null)
                        throw new Exception("Error Selecting From Orders");

                    orders.Add(order);
                }

                return orders;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error retrieving Orders list");
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<OrderEntityADO?> FindByIdAsync(int id)
        {
            try
            {
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@OrderID", System.Data.SqlDbType.Int, id)
                };

                var cmd = $"SELECT * FROM Orders WHERE OrderID = @OrderID";
                using SqlDataReader? reader = await RepoHelpers.ConnectAndGetReader(
                                                connectionFactory,
                                                cmd,
                                                parameters);

                //Reader Parsing
                await reader.ReadAsync();
                return GetOrderFromReader(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, $"Error retrieving admin with id: {id}", ex);
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region UPDATE_ROUTES
        public async Task<int> UpdateAsync(int id, OrderEntityADO entity)
        {
            //TODO: THIS NEEDS TO BE A TRANSACTION!
            try
            {
                object P(object? v) => v ?? DBNull.Value;
                //Setting up the command and parameters
                OrderDTO dto = OrderMapper.ToDto(entity);
                //Connection Setup & Execution
                var parameters = new List<SqlParameterModel>
                {
                    new("@OrderID", System.Data.SqlDbType.Int, id),
                    new("@CustomerID", System.Data.SqlDbType.Int, P(dto.CustomerId)),
                    new("@OrderDate", System.Data.SqlDbType.DateTime, dto.OrderDate),
                    new("@ShipAmount", System.Data.SqlDbType.Money, dto.ShipAmount),
                    new("@TaxAmount", System.Data.SqlDbType.Money,dto.TaxAmount),
                    new("@ShipDate", System.Data.SqlDbType.DateTime, P(dto.ShipDate)),
                    new("@ShipAddressID", System.Data.SqlDbType.Int, dto.ShipAddressId),
                    new("@CardType", System.Data.SqlDbType.VarChar, dto.CardType),
                    new("@CardNumber", System.Data.SqlDbType.VarChar, dto.CardNumber),
                    new("@CardExpires", System.Data.SqlDbType.VarChar, dto.CardExpires),
                    new("@BillingAddressID", System.Data.SqlDbType.Int, dto.BillingAddressId)
                };

                const string cmd = @"UPDATE Orders
                                        SET CustomerID = @CustomerID,
                                            OrderDate = @OrderDate,
                                            ShipAmount = @ShipAmount,
                                            TaxAmount = @TaxAmount,
                                            ShipDate = @ShipDate,
                                            ShipAddressID = @ShipAddressID,
                                            CardType = @CardType,
                                            CardNumber = @CardNumber,
                                            CardExpires = @CardExpires,
                                            BillingAddressID = @BillingAddressID
                                        WHERE OrderID = @OrderID";

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
                    new("@OrderID", System.Data.SqlDbType.Int, id)
                };

                var cmd = @"DELETE Orders 
                                WHERE OrderID = @OrderID";

                //Execute the command
                return await RepoHelpers.ConnectAndExecuteNonQuery(connectionFactory, cmd, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the Order");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region HELPERS
        private SqlParameter ToSqlParameter(SqlParameterModel model)
        {
            if (model == null) throw new ArgumentException(nameof(model));

            var param = new SqlParameter(model.Name, model.Type);

            param.Value = model.Value ?? DBNull.Value;
            return param;
        }
        private void AddParameters(SqlCommand cmd, IEnumerable<SqlParameterModel> parameters)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(ToSqlParameter(param));
            }
        }

        private OrderEntityADO? GetOrderFromReader(SqlDataReader reader)
        {
            try
            {
                int custOrdinal = reader.GetOrdinal("CustomerID");
                int shipOrdinal = reader.GetOrdinal("ShipDate");

                var order = new OrderEntityADO()
                {
                    OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                    CustomerID = reader.IsDBNull(custOrdinal) ? null : reader.GetInt32(custOrdinal),
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                    ShipAmount = reader.GetDecimal(reader.GetOrdinal("ShipAmount")),
                    TaxAmount = reader.GetDecimal(reader.GetOrdinal("TaxAmount")),
                    ShipDate = reader.IsDBNull(shipOrdinal) ? null : reader.GetDateTime(shipOrdinal),
                    ShipAddressID = reader.GetInt32(reader.GetOrdinal("ShipAddressID")),
                    CardType = reader.GetString(reader.GetOrdinal("CardType")),
                    CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
                    CardExpires = reader.GetString(reader.GetOrdinal("CardExpires")),
                    BillingAddressID = reader.GetInt32(reader.GetOrdinal("BillingAddressID"))
                };

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}