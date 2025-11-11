/**
 * @file    CustomerEntityADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an ADO Entity Representitive of a value from
 *              the 'Customers' Table of MyGuitarShop DB.
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
    /// An ADO Entity representing a Customer.
    /// </summary>
    public class CustomerEntityADO
    {
        [Key]
        public int CustomerID { get; set; }

        /// <summary>
        /// Unique
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string EmailAddress { get; set; }    

        [Required]
        [MaxLength(60)]
        public string Password { get; set; }

        [Required]
        [MaxLength(60)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(60)]
        public string LastName { get; set; }

        /// <summary>
        /// FK, Nullable
        /// </summary>
        public int? ShippingAddressID { get; set; }

        /// <summary>
        /// FK, Nullable
        /// </summary>
        public int? BillingAddressID { get; set; }  //FK

    }
}
