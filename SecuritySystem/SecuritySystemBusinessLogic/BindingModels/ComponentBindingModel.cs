using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.BindingModels
{
    // Компонент, требуемый для изготовления изделия

    public class ComponentBindingModel
    {
        public int? Id { get; set; }
       
        public string ComponentName { get; set; }
    }
}
