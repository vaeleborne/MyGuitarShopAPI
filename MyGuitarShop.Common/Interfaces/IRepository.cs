using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> FindByIdAsync(int id);
        Task<int> InsertAsync(T dto);
        Task<int> UpdateAsync(int id, T dto);
        Task<int> DeleteAsync(int id);
    }
}
