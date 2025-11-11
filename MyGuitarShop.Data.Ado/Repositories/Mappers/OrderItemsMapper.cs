using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class OrderItemsMapper
    {
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
