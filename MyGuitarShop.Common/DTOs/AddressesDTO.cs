using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyGuitarShop.Common.DTOs
{
    public class AddressesDTO
    {
        [Key]
        public int? AddressId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CustomerId Must Be A Positive Integer!")]
        public int? CustomerId { get; set; }

        [Required]
        [StringLength(60)]
        public string Line1 { get; set; }

        [StringLength(60)]
        public string? Line2 { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(12)]
        public string Phone {  get; set; }

        [Required]
        public int Disabled {  get; set; }

        [ForeignKey(nameof(CustomerId))]
        public CustomerDTO? Customer { get; set; }

    }
}
