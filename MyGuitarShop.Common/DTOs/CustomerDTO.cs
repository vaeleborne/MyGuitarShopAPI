using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class CustomerDTO
    {
        [Key]
        public int? CustomerId { get; set; }

        [Required]
        [StringLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(60)]
        public string Password { get; set; }

        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(60)]
        public string LastName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="ShippingAddressId Must Be A Positive Integer!")]
        public int? ShippingAddressId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BillingAddressId Must Be A Positive Integer!")]
        public int? BillingAddressId { get; set; }
    }
}
