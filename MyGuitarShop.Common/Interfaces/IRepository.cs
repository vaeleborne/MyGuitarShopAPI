using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<T>
    {
        #region CREATE_ROUTES
        Task<int> InsertAsync(T entity);
        #endregion

        #region READ_ROUTES
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindByIdAsync(int id);
        #endregion

        #region UPDATE_ROUTES
        Task<int> UpdateAsync(int id, T entity);
        #endregion

        #region DELETE_ROUTES
        Task<int> DeleteAsync(int id);

        #endregion
    }
}
