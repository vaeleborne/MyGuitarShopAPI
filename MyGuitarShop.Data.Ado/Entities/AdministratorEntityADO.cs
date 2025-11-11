/**
 * @file    AdministratorEntityADO.cs
 * @author  Dylan Hawke (Morgan)
 * @brief   Defines an ADO Entity Representitive of a value from
 *              the 'Administrators' Table of MyGuitarShop DB.
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
    /// An ADO Entity representing an Administrator.
    /// </summary>
    public class AdministratorEntityADO
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
    }
}
