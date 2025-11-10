using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Data.Ado.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories.Mappers
{
    public class AdministratorMapper
    {
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
