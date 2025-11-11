/**
 * @file    ProductEntityADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an ADO Entity Representitive of a value from
 *              the 'Products' Table of MyGuitarShop DB.
 * @date    2025-11-11
 * @version 1.0
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    /// <summary>
    /// An ADO Entity representing a Product.
    /// </summary>
    public class ProductEntityADO
    {
        [Key]
        public  int ProductID { get; set; }

        /// <summary>
        /// FK, Nullable
        /// </summary>
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
