/**
 * @file    AddressMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'Address' 
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
    /// Mapper class for converting Address DTOs and Entities.
    /// </summary>
    public class AddressMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The Address Entity To Convert</param>
        /// <returns>An Address DTO</returns>
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

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The Address DTO To Convert</param>
        /// <returns>An Address Entity</returns>
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

