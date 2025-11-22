using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<T, in TKey>
    {
        #region CREATE_ROUTES
        Task<bool> InsertAsync(T entity);
        #endregion

        #region READ_ROUTES
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindByIdAsync(TKey id);
        #endregion

        #region UPDATE_ROUTES
        Task<bool> UpdateAsync(TKey id, T entity);
        #endregion

        #region DELETE_ROUTES
        Task<bool> DeleteAsync(TKey id);

        #endregion
    }
}
