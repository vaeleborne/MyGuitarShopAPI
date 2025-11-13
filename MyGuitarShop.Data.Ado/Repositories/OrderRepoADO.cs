/**
 * @file    OrderRepoADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an OrderRepo for ADO that implements
 *              CRUD services for the Orders Table, to be used
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
    public class OrderRepoADO(
        ILogger<OrderRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<OrderEntityADO>
    {
        #region CREATE_ROUTES
        /// <summary>
        /// Implementation of the CREATE service to insert a new Order into the DB via a Transaction.
        /// This will also create OrderItems 
        /// </summary>
        /// <param name="entity">The Order entity to insert</param>
        /// <returns>The id of the newly connected</returns>
        /// <exception cref="Exception">Logs exception then throws to caller.</exception>
        public async Task<int> InsertAsync(OrderEntityADO entity)
        {      
            try
            {
                //Setup Connection For Transaction
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();
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

        /// <summary>
        /// Implementation of a READ service to get all orders from the DB.
        /// </summary>
        /// <returns>A list of OrderEntities representing all the Orders in the DB. </returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
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

        /// <summary>
        /// Implementation of a READ service to get an Order from its ID.
        /// </summary>
        /// <param name="id">The id of the Order to find.</param>
        /// <returns>The Order Entity Corresponding to {id}, or null if no such product is found.</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
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
        /// <summary>
        /// Implementation of an UPDATE service to update an Order via a Transaction, this will
        /// update the associated OrderItems by first Deleting all of them for the OrderID, then 
        /// creating/recreating the new OrderItems
        /// </summary>
        /// <param name="id">Id of the Order To update.</param>
        /// <param name="entity">Order Entity representing what the changes should be.</param>
        /// <returns>Number of items updates (success should be 1)</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<int> UpdateAsync(int id, OrderEntityADO entity)
        {
            //TODO: THIS NEEDS TO BE A TRANSACTION!
            try
            {

                await using var conn = await connectionFactory.OpenSqlConnectionAsync();

                using var transaction = conn.BeginTransaction();

                try
                {
                    //Local Helper Function
                    object P(object? v) => v ?? DBNull.Value;

                    //First, Update Orders:
                    using (var updateCmd = conn.CreateCommand())
                    {
                        updateCmd.Transaction = transaction;
                        updateCmd.CommandText = @"  UPDATE Orders
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
                                                     WHERE OrderID = @OrderID;";

                        updateCmd.Parameters.Add(new SqlParameter("@OrderID", System.Data.SqlDbType.Int) { Value = id });
                        updateCmd.Parameters.Add(new SqlParameter("@CustomerID", System.Data.SqlDbType.Int) { Value = P(entity.CustomerID) });
                        updateCmd.Parameters.Add(new SqlParameter("@OrderDate", System.Data.SqlDbType.DateTime) { Value = entity.OrderDate });

                 
                        var pShip = new SqlParameter("@ShipAmount", System.Data.SqlDbType.Money) { Value = entity.ShipAmount };
                        var pTax = new SqlParameter("@TaxAmount", System.Data.SqlDbType.Money) { Value = entity.TaxAmount };
                        updateCmd.Parameters.Add(pShip);
                        updateCmd.Parameters.Add(pTax);

                        updateCmd.Parameters.Add(new SqlParameter("@ShipDate", System.Data.SqlDbType.DateTime) { Value = P(entity.ShipDate) });
                        updateCmd.Parameters.Add(new SqlParameter("@ShipAddressID", System.Data.SqlDbType.Int) { Value = entity.ShipAddressID });
                        updateCmd.Parameters.Add(new SqlParameter("@CardType", System.Data.SqlDbType.VarChar) { Value = entity.CardType });
                        updateCmd.Parameters.Add(new SqlParameter("@CardNumber", System.Data.SqlDbType.VarChar) { Value = entity.CardNumber });
                        updateCmd.Parameters.Add(new SqlParameter("@CardExpires", System.Data.SqlDbType.VarChar) { Value = entity.CardExpires });
                        updateCmd.Parameters.Add(new SqlParameter("@BillingAddressID", System.Data.SqlDbType.Int) { Value = entity.BillingAddressID });

                        var rowsUpdated = await updateCmd.ExecuteNonQueryAsync();

                        // If rowsUpdated == 0 then,  treat as NotFound / throw
                        if (rowsUpdated == 0)
                            throw new InvalidOperationException($"Order with id {id} not found or nothing could be updated.");

                        //Replace OrderItems
                        
                        //First, delete the existing OrderItems
                        using (var delCmd = conn.CreateCommand())
                        {
                            delCmd.Transaction = transaction;
                            delCmd.CommandText = "DELETE FROM OrderItems WHERE OrderID = @OrderID";
                            delCmd.Parameters.Add(new SqlParameter("@OrderID", System.Data.SqlDbType.Int) { Value = id });
                            await delCmd.ExecuteNonQueryAsync();
                        }

                        //Now, Insert OrderItems, if any exists
                        if (entity.Items != null && entity.Items.Any())
                        {
                            using var itemCmd = conn.CreateCommand();
                            itemCmd.Transaction = transaction;
                            itemCmd.CommandText = @"    INSERT INTO OrderItems 
                                                        (
                                                            OrderID, 
                                                            ProductID, 
                                                            ItemPrice, 
                                                            DiscountAmount, 
                                                            Quantity
                                                        )
                                                        VALUES 
                                                        (
                                                            @OrderID, 
                                                            @ProductID, 
                                                            @ItemPrice, 
                                                            @DiscountAmount, 
                                                            @Quantity
                                                        );";

                            // Create parameters to be reused within a loop
                            itemCmd.Parameters.Add(new SqlParameter("@OrderID", System.Data.SqlDbType.Int));
                            itemCmd.Parameters.Add(new SqlParameter("@ProductID", System.Data.SqlDbType.Int));
                            var pItemPrice = new SqlParameter("@ItemPrice", System.Data.SqlDbType.Money);
                            var pDiscount = new SqlParameter("@DiscountAmount", System.Data.SqlDbType.Money);
                            var pQty = new SqlParameter("@Quantity", System.Data.SqlDbType.Int);
                            itemCmd.Parameters.Add(pItemPrice);
                            itemCmd.Parameters.Add(pDiscount);
                            itemCmd.Parameters.Add(pQty);

                            foreach (var item in entity.Items)
                            {
                                // Simple Validation
                                if (item.ProductID <= 0) throw new ArgumentException("OrderItem.ProductID must be > 0");
                                if (item.Quantity <= 0) throw new ArgumentException("OrderItem.Quantity must be > 0");

                                itemCmd.Parameters["@OrderID"].Value = id;
                                itemCmd.Parameters["@ProductID"].Value = item.ProductID;
                                itemCmd.Parameters["@ItemPrice"].Value = item.ItemPrice;
                                itemCmd.Parameters["@DiscountAmount"].Value = item.DiscountAmount;
                                itemCmd.Parameters["@Quantity"].Value = item.Quantity;

                                await itemCmd.ExecuteNonQueryAsync();
                            }
                        }

                        //Success! Commit Transaction
                        transaction.Commit();
                        return rowsUpdated;
                    }
                }
                catch (Exception transactEx)
                {
                    transaction.Rollback();
                    throw new Exception(transactEx.Message, transactEx);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error updating the Category!");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region DELETE_ROUTES
        /// <summary>
        /// Implements a DELETE service to delete an Order from the DB.
        /// </summary>
        /// <param name="id">Id of the Order to delete.</param>
        /// <returns>Number of items deleted (success should be 1).</returns>
        /// <exception cref="Exception">Logs then throws to caller.</exception>
        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                await using var conn = await connectionFactory.OpenSqlConnectionAsync();
                using var transaction = conn.BeginTransaction();

                try
                {
                    //Local Helper
                    object P(object? v) => v ?? DBNull.Value;

                    //Delete the OrderItems for the order
                    int itemsDeleted = 0;
                    await using (var delItemsCmd = conn.CreateCommand())
                    {
                        delItemsCmd.Transaction = transaction;
                        delItemsCmd.CommandText = "DELETE OrderItems WHERE OrderID = @OrderID";
                        delItemsCmd.Parameters.Add(new SqlParameter("@OrderID", System.Data.SqlDbType.Int) { Value = id });

                        itemsDeleted = await delItemsCmd.ExecuteNonQueryAsync();
                    }

                    //Delete the Order itself
                    int ordersDeleted = 0;
                    await using (var delOrderCmd = conn.CreateCommand())
                    {
                        delOrderCmd.Transaction = transaction;
                        delOrderCmd.CommandText = "DELETE Orders WHERE OrderID = @OrderID";
                        delOrderCmd.Parameters.Add(new SqlParameter("@OrderID", System.Data.SqlDbType.Int) { Value = id });

                        ordersDeleted = await delOrderCmd.ExecuteNonQueryAsync();
                    }

                    //Commit changes
                    transaction.Commit();

                    return ordersDeleted;
                }
                catch (Exception transactEx)
                {
                    transaction.Rollback();
                    logger.LogError("Unable To Delete Order!");
                    throw new Exception(transactEx.Message, transactEx);
                } 
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Error deleting the Order");
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// Converts a SqlParameterModel to a SqlParameter.
        /// </summary>
        /// <param name="model">The SqlParameterModel to convert.</param>
        /// <returns>The SqlParameter from the model.</returns>
        /// <exception cref="ArgumentException">Logs then throws to caller.</exception>
        private SqlParameter ToSqlParameter(SqlParameterModel model)
        {
            if (model == null) throw new ArgumentException(nameof(model));

            var param = new SqlParameter(model.Name, model.Type);

            param.Value = model.Value ?? DBNull.Value;
            return param;
        }

        /// <summary>
        /// Adds parameters to a SqlCommand
        /// </summary>
        /// <param name="cmd">The command to add parameters to.</param>
        /// <param name="parameters">An enumarable of SqplParameterModels representing the parameters to add.</param>
        private void AddParameters(SqlCommand cmd, IEnumerable<SqlParameterModel> parameters)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(ToSqlParameter(param));
            }
        }

        /// <summary>
        /// Gets an Order from a reader.
        /// </summary>
        /// <param name="reader">The reader to parse for the Order.</param>
        /// <returns>An Order, or Null if none exists.</returns>
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