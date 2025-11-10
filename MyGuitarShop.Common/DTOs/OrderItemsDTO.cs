using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class OrderItemsDTO
    {
        [Key]
        public int? ItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "OrderId Must Be A Positive Integer!")]
        public int? OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ProductId Must Be A Positive Integer!")]
        public int? ProductId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal ItemPrice { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal DiscountAmount { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity Must Be A Positive Integer!")]
        public int Quantity { get; set; }


        [ForeignKey(nameof(OrderId))]
        public OrderDTO? Order { get ; set; }

        [ForeignKey(nameof(ProductId))]
        public ProductDTO? Product { get; set; }
    }
}
