using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuritySystemDatabaseImplement.Models
{
    public class SecureComponent
    {
        public int Id { get; set; }

        public int SecureId { get; set; }

        public int ComponentId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Component Component { get; set; }

        public virtual Secure Secure { get; set; }
    }
}
