using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public class StoreHouseLogic
    {
        private readonly IStoreHouseStorage _storeHouseStorage;

        private readonly IComponentStorage _componentStorage;
        
        public StoreHouseLogic(IStoreHouseStorage storeHouseStorage, IComponentStorage componentStorage)
        {
            _storeHouseStorage = storeHouseStorage;
            _componentStorage = componentStorage;
        }



        public List<StoreHouseViewModel> Read(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return _storeHouseStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<StoreHouseViewModel> { _storeHouseStorage.GetElement(model)};
            }
            return _storeHouseStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(StoreHouseBindingModel model)
        {
            var element = _storeHouseStorage.GetElement(new StoreHouseBindingModel
            {
                StoreHouseName = model.StoreHouseName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            if (model.Id.HasValue)
            {
                _storeHouseStorage.Update(model);
            }
            else
            {
                _storeHouseStorage.Insert(model);
            }
        }

        public void Delete(StoreHouseBindingModel model)
        {
            var element = _storeHouseStorage.GetElement(new StoreHouseBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _storeHouseStorage.Delete(model);
        }

        public void Replenishment(StoreHouseReplenishmentBindingModel model)
        {
            var storeHouse = _storeHouseStorage.GetElement(new StoreHouseBindingModel
            {
                Id = model.StoreHouseId
            });

            var component = _componentStorage.GetElement(new ComponentBindingModel
            {
                Id = model.ComponentId
            });
            if (storeHouse == null)
            {
                throw new Exception("Не найден склад");
            }
            if (component == null)
            {
                throw new Exception("Не найден компонент");
            }
            if (storeHouse.StoreHouseComponents.ContainsKey(model.ComponentId))
            {
                storeHouse.StoreHouseComponents[model.ComponentId] = 
                    (component.ComponentName, storeHouse.StoreHouseComponents[model.ComponentId].Item2 + model.Count);
            }
            else
            {
                storeHouse.StoreHouseComponents.Add(component.Id, (component.ComponentName, model.Count));
            }
            _storeHouseStorage.Update(new StoreHouseBindingModel
            {
                Id = storeHouse.Id,
                StoreHouseName = storeHouse.StoreHouseName,
                ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                DateCreate = storeHouse.DateCreate,
                StoreHouseComponents = storeHouse.StoreHouseComponents
            });
        }
    }
}
