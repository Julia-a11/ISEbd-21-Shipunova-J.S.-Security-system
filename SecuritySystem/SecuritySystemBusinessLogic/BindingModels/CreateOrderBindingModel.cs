using System.Runtime.Serialization;

namespace SecuritySystemBusinessLogic.BindingModels
{
    // Данные от клиента, для создания заказа
    [DataContract]
    public class CreateOrderBindingModel
    {
        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int SecureId { get; set; }
       
        [DataMember]
        public int Count { get; set; }
      
        [DataMember]
        public decimal Sum { get; set; }
    }
}