using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class ProductEntityADO
    {
        [Key]
        public  int ProductID { get; set; }
        public int? CategoryID { get; set; }

        [Required]
        [MaxLength(10)]
        public  string ProductCode { get; set; }

        [Required]
        [MaxLength(255)]
        public  string ProductName {  get; set; }

        [Required]
        public  string Description { get; set; }

        [Required]
        public  decimal? ListPrice { get; set; }

        [Required]
        public  decimal? DiscountPercent { get; set; }

        public DateTime? DateAdded { get; set; }
    }
}
