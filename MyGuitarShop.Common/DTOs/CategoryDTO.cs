using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyGuitarShop.Common.DTOs
{
    public class CategoryDTO
    {
        [Key]
        public int? CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }
    }
}
