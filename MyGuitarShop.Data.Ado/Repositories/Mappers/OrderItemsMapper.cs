/**
 * @file    OrderItemsMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'OrderItems' 
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
    /// Mapper class for converting OrderItem DTOs and Entities.
    /// </summary>
    public class OrderItemsMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The OrderItem Entity To Convert</param>
        /// <returns>An OrderItem DTO</returns>
        public static OrderItemsDTO ToDto(OrderItemEntityADO entity)
        {
            return new OrderItemsDTO()
            {
                ItemId = entity.ItemID,
                OrderId = entity.OrderID,
                ProductId = entity.ProductID,
                ItemPrice = entity.ItemPrice,
                DiscountAmount = entity.DiscountAmount,
                Quantity = entity.Quantity
            };
        }

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The OrderItem DTO To Convert</param>
        /// <returns>An OrderItem Entity</returns>
        public static OrderItemEntityADO ToEntity(OrderItemsDTO dto)
        {
            dto.ItemId ??= 0;
            dto.OrderId ??= 0;
            dto.ProductId ??= 0;

            return new OrderItemEntityADO()
            {
                ItemID = dto.ItemId.Value,
                OrderID = dto.OrderId.Value,
                ProductID = dto.ProductId.Value,
                ItemPrice = dto.ItemPrice,
                DiscountAmount = dto.DiscountAmount,
                Quantity= dto.Quantity
            };
        }
    }
}
