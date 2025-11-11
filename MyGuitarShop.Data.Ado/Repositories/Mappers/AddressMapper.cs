using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class AddressMapper
    {
        public static AddressesDTO ToDto(AddressEntityADO entity)
        {
            return new AddressesDTO()
            {
                AddressId = entity.AddressID,
                CustomerId = entity.CustomerID,
                Line1 = entity.Line1,
                Line2 = entity.Line2,
                City = entity.City,
                State = entity.State,
                ZipCode = entity.ZipCode,
                Phone = entity.Phone,
                Disabled = entity.Disabled
            };
        }

        public static AddressEntityADO ToEntity(AddressesDTO dto)
        {
            dto.AddressId ??= 0;

            return new AddressEntityADO()
            {
                AddressID = dto.AddressId.Value,
                CustomerID = dto.CustomerId,
                Line1 = dto.Line1,
                Line2 = dto.Line2,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Phone = dto.Phone,
                Disabled = dto.Disabled
            };
        }
    }
}

