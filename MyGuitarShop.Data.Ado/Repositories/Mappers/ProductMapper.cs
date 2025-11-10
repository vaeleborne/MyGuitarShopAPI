using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class ProductMapper
    {
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
