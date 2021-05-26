using SecuritySystemBusinessLogic.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SecuritySystemBusinessLogic.ViewModels
{
    // Изделие, изготавливаемое в магазине
    [DataContract]
    public class SecureViewModel
    {
        [Column(title: "Номер", width: 100)]
        [DataMember]
        public int Id { get; set; }

        [Column(title: "Название комплектации",gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string SecureName { get; set; }

        [Column(title: "Цена", width: 100)]
        [DataMember]
        public decimal Price { get; set; }

        [Column(visible: false)]
        [DataMember]
        public Dictionary<int, (string, int)> SecureComponents { get; set; }
    }
}