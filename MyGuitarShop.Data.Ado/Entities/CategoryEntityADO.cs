/**
 * @file    CategoryEntityADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an ADO Entity Representitive of a value from
 *              the 'Categories' Table of MyGuitarShop DB.
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
    /// An ADO Entity representing a Category.
    /// </summary>
    public class CategoryEntityADO
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }
    }
}
