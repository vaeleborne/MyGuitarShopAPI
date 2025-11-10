using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{

    public class ProductDTO
    {
        [Key]
        public int? ProductID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CategoryId Must Be A Positive Integer!")]
        public int? CategoryID { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName  { get; set; }

        [StringLength(10)]
        public string? ProductCode { get; set; }

        [DataType(DataType.Currency)]
        public decimal? ListPrice { get; set; }


        public decimal? DiscountPercent { get; set; }

        public string? Description { get; set; }

        public DateTime? DateAdded { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public CategoryDTO? Category { get; set; }

    }
}
