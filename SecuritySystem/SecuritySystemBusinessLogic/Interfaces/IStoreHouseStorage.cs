using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.Interfaces
{
    public interface IStoreHouseStorage
    {
         List<StoreHouseViewModel> GetFullList();
        
        List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model);
       
        StoreHouseViewModel GetElement(StoreHouseBindingModel model);
        
        void Insert(StoreHouseBindingModel model);
       
        void Update(StoreHouseBindingModel model);
       
        void Delete(StoreHouseBindingModel model);
    }
}
