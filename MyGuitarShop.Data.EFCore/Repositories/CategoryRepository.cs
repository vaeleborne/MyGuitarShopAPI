
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGuitarShop.Data.EFCore.Context;
using MyGuitarShop.Data.EFCore.Entities;

namespace MyGuitarShop.Data.EFCore.Repositories
{
    public class CategoryRepository(MyGuitarShopContext dbContext) : RepositoryBase<Category>(dbContext)
    {

    }
}
