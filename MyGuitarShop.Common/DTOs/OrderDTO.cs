using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class OrderDTO
    {
        [Key]
        public int? OrderId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId Must Be A Positive Integer!")]
        public int? CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "ShipAmount")]
        public decimal ShipAmount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "TaxAmount")]
        public decimal TaxAmount { get; set; }

        public DateTime? ShipDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ShipAddressId Must Be A Positive Integer!")]
        public int ShipAddressId { get; set; }

        [Required]
        [StringLength(50)]
        public string CardType { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "CardNumber must be exactly 16 characters long!")]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "CardExpires must be exactly 7 characters long!")]
        public string CardExpires { get; set; }

        public List<OrderItemsDTO?> Items { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "BillingAddressId Must Be A Positive Integer!")]
        public int BillingAddressId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public CustomerDTO? Customer {  get; set; }


    }
}
