using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.ViewModels;
using System.Collections.Generic;

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