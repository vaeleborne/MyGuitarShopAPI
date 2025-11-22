using MyGuitarShop.Common.Enums;
using MyGuitarShop.Data.MongoDb.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.MongoDb.Models
{
    public class ProductModel : MongoModel
    {
        public CategoryType Category { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public decimal ListPrice { get; set; }

        public decimal DiscountPercent { get; set; }

        public DateTime DateAdded { get; init; }  = DateTime.UtcNow;
    }
}
