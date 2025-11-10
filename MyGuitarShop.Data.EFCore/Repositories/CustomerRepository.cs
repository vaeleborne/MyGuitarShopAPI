
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyGuitarShop.Data.EFCore.Context;
using MyGuitarShop.Data.EFCore.Entities;

namespace MyGuitarShop.Data.EFCore.Repositories
{
    public class CustomerRepository(MyGuitarShopContext dbContext) : RepositoryBase<Customer>(dbContext)
    {
        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Include(customer => customer.Addresses)
                .Include(customer => customer.Orders)
                .ToListAsync();
        }
    }
}
