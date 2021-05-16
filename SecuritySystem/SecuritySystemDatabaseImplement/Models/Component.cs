﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecuritySystemDatabaseImplement.Models
{
    public class Component
    {
        public int Id { get; set; }

        [Required]
        public string ComponentName { get; set; }

        [ForeignKey("ComponentId")]
        public virtual List<SecureComponent> SecureComponents { get; set; }

        [ForeignKey("ComponentId")]
        public virtual List<StoreHouseComponent> StoreHouseComponents { get; set; }
    }
}