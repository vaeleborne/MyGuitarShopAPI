/**
 * @file    CustomerMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'Customer' 
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
    /// Mapper class for converting Customer DTOs and Entities.
    /// </summary>
    public class CustomerMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The Customer Entity To Convert</param>
        /// <returns>A Customer DTO</returns>
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

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The Customer DTO To Convert</param>
        /// <returns>A Customer Entity</returns>
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
