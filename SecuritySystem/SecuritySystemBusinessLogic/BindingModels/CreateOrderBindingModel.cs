namespace SecuritySystemBusinessLogic.BindingModels
{
    // Данные от клиента, для создания заказа
    public class CreateOrderBindingModel
    {
        public int SecureId { get; set; }
       
        public int Count { get; set; }
      
        public decimal Sum { get; set; }
    }
}