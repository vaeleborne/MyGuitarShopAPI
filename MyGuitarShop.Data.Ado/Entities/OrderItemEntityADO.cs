/**
 * @file    OrderItemEntityADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an ADO Entity Representitive of a value from
 *              the 'OrderItems' Table of MyGuitarShop DB.
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
    /// An ADO Entity representing an OrderItem.
    /// </summary>
    public class OrderItemEntityADO
    {
        [Key]
        public int ItemID { get; set; }

        /// <summary>
        /// FK
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// FK
        /// </summary>
        public int ProductID { get; set; }

        [Required]
        public decimal ItemPrice { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}
