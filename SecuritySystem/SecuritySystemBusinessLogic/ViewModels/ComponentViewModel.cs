using SecuritySystemBusinessLogic.Attributes;
using System.ComponentModel;

namespace SecuritySystemBusinessLogic.ViewModels
{
    // Компонент, требуемый для изготовления изделия

    public class ComponentViewModel
    {
        [Column(title: "Номер", width: 100)]
        public int Id { get; set; }

        [Column(title: "Название компонента", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string ComponentName { get; set; }
    }
}