using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SecuritySystemDatabaseImplement.Models
{
    public class StoreHouseComponent
    {
        public int Id { get; set; }

        public int StoreHouseId { get; set; }

        public int ComponentId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Component Component { get; set; }

        public virtual StoreHouse StoreHouse { get; set; }
    }
}
