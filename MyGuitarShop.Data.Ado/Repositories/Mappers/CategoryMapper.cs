using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class CategoryMapper
    {
        public static CategoryDTO ToDto(CategoryEntityADO entity)
        {
            return new CategoryDTO()
            {
                CategoryId = entity.CategoryID,
                CategoryName = entity.CategoryName
            };
        }

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
