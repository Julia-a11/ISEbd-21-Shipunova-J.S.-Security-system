using System.Collections.Generic;

namespace SecuritySystemListImplement.Models
{
    // Комплектация, изготавливаемая в магазине
    public class Secure
    {
        public int Id { get; set; }

        public string SecureName { get; set; }

        public decimal Price { get; set; }

        public Dictionary<int, int> SecureComponents { get; set; }
    }
}
