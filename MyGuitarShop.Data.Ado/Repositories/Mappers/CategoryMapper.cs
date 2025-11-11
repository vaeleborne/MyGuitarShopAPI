/**
 * @file    CategoriesMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'Category' 
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
    /// Mapper class for converting Category DTOs and Entities.
    /// </summary>
    public class CategoryMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The Category Entity To Convert</param>
        /// <returns>A Category DTO</returns>
        public static CategoryDTO ToDto(CategoryEntityADO entity)
        {
            return new CategoryDTO()
            {
                CategoryId = entity.CategoryID,
                CategoryName = entity.CategoryName
            };
        }

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The Category DTO To Convert</param>
        /// <returns>A Category Entity</returns>
        public static CategoryEntityADO ToEntity(CategoryDTO dto)
        {
            dto.CategoryId ??= 0;

            return new CategoryEntityADO()
            {
                CategoryID = dto.CategoryId.Value,
                CategoryName = dto.CategoryName
            };
        }
    }
}
