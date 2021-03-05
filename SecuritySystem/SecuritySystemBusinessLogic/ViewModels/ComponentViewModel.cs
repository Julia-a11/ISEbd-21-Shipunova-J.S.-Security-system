using System.ComponentModel;

namespace SecuritySystemBusinessLogic.ViewModels
{
    // Компонент, требуемый для изготовления изделия

    public class ComponentViewModel
    {
        public int Id { get; set; }
        
        [DisplayName("Название компонента")]
        public string ComponentName { get; set; }
    }
}