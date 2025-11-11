using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class OrderItemEntityADO
    {
        [Key]
        public int ItemID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        [Required]
        public decimal ItemPrice { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}
