using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SecuritySystemDatabaseImplement.Models
{
    public class Secure
    {
        public int Id { get; set; }

        [Required]
        public string SecureName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("SecureId")]
        public virtual List<SecureComponent> SecureComponents { get; set; }

        [ForeignKey("SecureId")]
        public virtual List<Order> Orders { get; set; }
    }
}
