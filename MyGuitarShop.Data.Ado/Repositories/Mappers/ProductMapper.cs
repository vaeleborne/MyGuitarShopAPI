/**
 * @file    ProductMapper.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines a class to convert 'Product' 
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
    /// Mapper class for converting Product DTOs and Entities.
    /// </summary>
    public class ProductMapper
    {
        /// <summary>
        /// Converts an entity to a dto.
        /// </summary>
        /// <param name="entity">The Product Entity To Convert</param>
        /// <returns>A Product DTO</returns>
        public static ProductDTO ToDto(ProductEntityADO entity)
        {
            return new ProductDTO()
            {
                ProductID = entity.ProductID,
                CategoryID = entity.CategoryID,
                ProductCode = entity.ProductCode,
                ProductName = entity.ProductName,
                Description = entity.Description,
                ListPrice = entity.ListPrice,
                DiscountPercent = entity.DiscountPercent,
                DateAdded = entity.DateAdded,
            };
        }

        /// <summary>
        /// Converts a dto to an entity.
        /// </summary>
        /// <param name="dto">The Product DTO To Convert</param>
        /// <returns>A Product Entity</returns>
        public static ProductEntityADO ToEntity(ProductDTO dto)
        {
            dto.ProductID ??= 0;
            dto.DateAdded ??= DateTime.UtcNow;

            return new ProductEntityADO()
            {
                ProductID = dto.ProductID.Value,
                CategoryID = dto.CategoryID,
                ProductName = dto.ProductName,
                ProductCode = dto.ProductCode ?? "",
                ListPrice = dto.ListPrice ?? 0,
                DiscountPercent = dto.DiscountPercent ?? 0,
                Description = dto.Description ?? "",
                DateAdded = dto.DateAdded
            };
        }
    }
}
