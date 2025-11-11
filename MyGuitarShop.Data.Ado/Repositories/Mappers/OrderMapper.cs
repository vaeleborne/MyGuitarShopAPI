using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class OrderMapper
    {
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
