using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public class SecureLogic
    {
        private readonly ISecureStorage _secureStorage;
        public SecureLogic(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public List<SecureViewModel> Read(SecureBindingModel model)
        {
            if (model == null)
            {
                return _secureStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<SecureViewModel> { _secureStorage.GetElement(model)
};
            }
            return _secureStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(SecureBindingModel model)
        {
            var element = _secureStorage.GetElement(new SecureBindingModel
            {
                SecureName = model.SecureName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть комплектация с таким названием");
            }
            if (model.Id.HasValue)
            {
                _secureStorage.Update(model);
            }
            else
            {
                _secureStorage.Insert(model);
            }
        }

        public void Delete(SecureBindingModel model)
        {
            var element = _secureStorage.GetElement(new SecureBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _secureStorage.Delete(model);
        }
    }
}

