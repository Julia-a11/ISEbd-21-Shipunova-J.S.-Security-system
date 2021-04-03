using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SecuritySystemBusinessLogic.ViewModels
{
    // Изделие, изготавливаемое в магазине
    [DataContract]
    public class SecureViewModel
    {
        [DataMember]
        public int Id { get; set; }
       
        [DataMember]
        [DisplayName("Название комплектации")]
        public string SecureName { get; set; }
        
        [DataMember]
        [DisplayName("Цена")]
        public decimal Price { get; set; }
       
        [DataMember]
        public Dictionary<int, (string, int)> SecureComponents { get; set; }
    }
}