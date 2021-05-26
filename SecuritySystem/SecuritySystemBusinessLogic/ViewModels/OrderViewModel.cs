using SecuritySystemBusinessLogic.Attributes;
using SecuritySystemBusinessLogic.Enums;
using System;
using System.Runtime.Serialization;


namespace SecuritySystemBusinessLogic.ViewModels
{
    // Заказ
    [DataContract]
    public class OrderViewModel
    {
        [Column(title: "Номер", width: 50)]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int SecureId { get; set; }

        [DataMember]
        public int? ImplementerId { get; set; }

        [Column(title: "Клиент", width: 150)]
        [DataMember]
        public string ClientFIO { get; set; }

        [Column(title: "Комплектация охраны", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string SecureName { get; set; }

        [Column(title: "Исполнитель", width: 80)]
        [DataMember]
        public string ImplementerFIO { get; set; }

        [Column(title: "Количество", width: 60)]
        [DataMember]
        public int Count { get; set; }

        [Column(title: "Сумма", width: 50)]
        [DataMember]
        public decimal Sum { get; set; }

        [Column(title: "Статус", width: 100)]
        [DataMember]
        public OrderStatus Status { get; set; }

        [Column(title: "Дата создания", width: 100, format: "dd-MM-yyyy")]
        [DataMember]
        public DateTime DateCreate { get; set; }

        [Column(title: "Дата выполнения", width: 100, format: "dd-MM-yyyy")]
        [DataMember]
        public DateTime? DateImplement { get; set; }
    }
}
