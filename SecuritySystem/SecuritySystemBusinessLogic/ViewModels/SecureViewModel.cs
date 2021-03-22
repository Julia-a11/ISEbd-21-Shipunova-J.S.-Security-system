using System.Collections.Generic;
using System.ComponentModel;

namespace SecuritySystemBusinessLogic.ViewModels
{
    // Изделие, изготавливаемое в магазине
    public class SecureViewModel
    {
        public int Id { get; set; }
       
        [DisplayName("Название комплектации")]
        public string SecureName { get; set; }
        
        [DisplayName("Цена")]
        public decimal Price { get; set; }
       
        public Dictionary<int, (string, int)> SecureComponents { get; set; }
    }
}