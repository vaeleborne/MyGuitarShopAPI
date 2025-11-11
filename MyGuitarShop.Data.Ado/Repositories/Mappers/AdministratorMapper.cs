/**
 * @file    AdministratorMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'Administrator' 
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
    /// Mapper class for converting Administrator DTOs and Entities.
    /// </summary>
    public class AdministratorMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The Administrator Entity To Convert</param>
        /// <returns>An Administrator DTO</returns>
        public static AdministratorDTO ToDto(AdministratorEntityADO entity)
        {
            return new AdministratorDTO()
            {
                AdminId = entity.AdminId,
                EmailAddress = entity.EmailAddress,
                Password = entity.Password,
                FirstName = entity.FirstName,
                LastName = entity.LastName
            };
        }

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The Administrator DTO To Convert</param>
        /// <returns>An Administrator Entity</returns>
        public static AdministratorEntityADO ToEntity(AdministratorDTO dto)
        {
            dto.AdminId ??= 0;

            return new AdministratorEntityADO()
            {
                AdminId = dto.AdminId.Value,
                EmailAddress = dto.EmailAddress,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
        }
    }
}
