/**
 * @file    OrderMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'Order' 
 *              DTOs to Entities and vice versa.
 * @date    2025-11-11
 * @version 1.0
 */
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    /// <summary>
    /// Mapper class for converting Order DTOs and Entities.
    /// </summary>
    public class OrderMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The Order Entity To Convert</param>
        /// <returns>An Order DTO</returns>
        public static OrderDTO ToDto(OrderEntityADO entity)
        {
            return new OrderDTO()
            {
                OrderId = entity.OrderID,
                CustomerId = entity.CustomerID,
                OrderDate = entity.OrderDate,
                ShipAmount = entity.ShipAmount,
                TaxAmount = entity.TaxAmount,
                ShipDate = entity.ShipDate,
                ShipAddressId = entity.ShipAddressID,
                CardType = entity.CardType,
                CardNumber = entity.CardNumber,
                CardExpires = entity.CardExpires,
                BillingAddressId = entity.BillingAddressID
            };
        }

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The Order DTO To Convert</param>
        /// <returns>An Order Entity</returns>
        public static OrderEntityADO ToEntity(OrderDTO dto)
        {
            dto.OrderId ??= 0;

            return new OrderEntityADO()
            {
                OrderID = dto.OrderId.Value,
                CustomerID = dto.CustomerId,
                OrderDate = dto.OrderDate,
                ShipAmount = dto.ShipAmount,
                TaxAmount = dto.TaxAmount,
                ShipDate = dto.ShipDate,
                ShipAddressID = dto.ShipAddressId,
                CardType = dto.CardType,
                CardNumber = dto.CardNumber,
                CardExpires = dto.CardExpires,
                BillingAddressID = dto.BillingAddressId
            };
        }
    }
}
