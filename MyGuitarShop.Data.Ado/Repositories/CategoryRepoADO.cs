using Microsoft.Extensions.Logging;
using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Repositories
{
    public class CategoryRepoADO(
        ILogger<CategoryRepoADO> logger,
        SqlConnectionFactory connectionFactory
    )
    : IRepository<CategoryEntityADO>
    {
        #region CREATE_ROUTES
        public async Task<int> InsertAsync(CategoryEntityADO entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region READ_ROUTES
        public async Task<IEnumerable<CategoryEntityADO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<CategoryEntityADO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region UPDATE_ROUTES
        public async Task<int> UpdateAsync(int id, CategoryEntityADO entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DELETE_ROUTES
        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
