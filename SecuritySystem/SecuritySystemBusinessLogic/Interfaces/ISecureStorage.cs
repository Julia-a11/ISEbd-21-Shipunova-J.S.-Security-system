using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.Interfaces
{
    public interface ISecureStorage
    {
        List<SecureViewModel> GetFullList();
        
        List<SecureViewModel> GetFilteredList(SecureBindingModel model);
       
        SecureViewModel GetElement(SecureBindingModel model);
        
        void Insert(SecureBindingModel model);
       
        void Update(SecureBindingModel model);
       
        void Delete(SecureBindingModel model);
    }
}
