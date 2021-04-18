using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.Interfaces
{
    public interface IImplementerStorage
    {
        List<ImplementerViewModel> GetFullList();
       
        List<ImplementerViewModel> GetFilteredList(ImplementerBindingModel model);
        
        ImplementerViewModel GetElement(ImplementerBindingModel model);
       
        void Insert(ImplementerBindingModel model);
       
        void Update(ImplementerBindingModel model);
       
        void Delete(ImplementerBindingModel model);
    }
}
