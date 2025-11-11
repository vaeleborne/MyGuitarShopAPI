using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class CustomerMapper
    {
        public static CustomerDTO ToDto(CustomerEntityADO entity)
        {
            return new CustomerDTO()
            {
                CustomerId = entity.CustomerID,
                EmailAddress = entity.EmailAddress,
                Password = entity.Password,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                ShippingAddressId = entity.ShippingAddressID,
                BillingAddressId = entity.BillingAddressID
            };
        }

        public static CustomerEntityADO ToEntity(CustomerDTO dto)
        {
            dto.CustomerId ??= 0;

            return new CustomerEntityADO()
            {
                CustomerID = dto.CustomerId.Value,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ShippingAddressID = dto.ShippingAddressId,
                BillingAddressID = dto.BillingAddressId
            };
        }
    }
}
