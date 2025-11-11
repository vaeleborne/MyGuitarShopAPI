/**
 * @file    AddressEntityADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an ADO Entity Representitive of a value from
 *              the 'Addresses' Table of MyGuitarShop DB.
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
    /// An ADO Entity representing an Address.
    /// </summary>
    public class AddressEntityADO
    {
        [Key]
        public int AddressID { get; set; }

        /// <summary>
        /// FK, Nullable
        /// </summary>
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
