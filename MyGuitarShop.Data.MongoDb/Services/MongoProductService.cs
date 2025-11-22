using MongoDB.Driver;
using MyGuitarShop.Data.MongoDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.MongoDb.Services
{
    public class MongoProductService(IMongoDatabase database) : MongoService<ProductModel>(database)
    {

    }
}
