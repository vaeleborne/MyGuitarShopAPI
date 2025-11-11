using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class AddressEntityADO
    {
        [Key]
        public int AddressID { get; set; }

        public int? CustomerID { get; set; }

        [Required]
        [MaxLength(60)]
        public string Line1 { get; set; }

        [MaxLength(60)]
        public string? Line2 { get; set; }

        [Required]
        [MaxLength(40)]
        public string City { get; set; }

        [Required]
        [MaxLength(2)]
        public string State { get; set; }

        [Required]
        [MaxLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(12)]
        public string Phone { get; set; }

        [Required]
        public int Disabled {  get; set; }
    }
}
