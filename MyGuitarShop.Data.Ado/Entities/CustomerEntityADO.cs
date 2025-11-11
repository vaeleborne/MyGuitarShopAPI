using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class CustomerEntityADO
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(60)]
        public string Password { get; set; }

        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }

        public int? ShippingAddressID { get; set; }

        public int? BillingAddressID { get; set; }

    }
}
