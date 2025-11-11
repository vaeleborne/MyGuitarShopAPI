using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class OrderEntityADO
    {
        [Key]
        public int OrderID { get; set; }

        public int? CustomerID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal ShipAmount { get; set; }

        [Required]
        public decimal TaxAmount { get; set; }

        public DateTime? ShipDate { get; set; }

        [Required]
        public int ShipAddressID { get; set; }

        [Required]
        public string CardType { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string CardExpires { get; set; }

        [Required]
        public int BillingAddressID { get; set; }

        public List<OrderItemEntityADO>? Items { get; set; }
    }
}
