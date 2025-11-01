using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{

    public class ProductDTO
    {
        [Key]
        public int? ProductID { get; set; }
        
        public int? CategoryID { get; set; }

        [Required]
        public string ProductName  { get; set; }

        public string? ProductCode { get; set; }

        public decimal? ListPrice { get; set; }

        public decimal? DiscountPercent { get; set; }

        public string? Description { get; set; }

        public DateTime? DateAdded { get; set; }

    }
}
