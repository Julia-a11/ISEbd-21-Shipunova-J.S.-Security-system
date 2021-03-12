using System.Collections.Generic;

namespace SecuritySystemBusinessLogic.BindingModels
{
    // Комплектация, изготавливаемая в магазине
    public class SecureBindingModel
    {
        public int? Id { get; set; }
       
        public string SecureName { get; set; }
       
        public decimal Price { get; set; }
        
        public Dictionary<int, (string, int)> SecureComponents { get; set; }
    }
}