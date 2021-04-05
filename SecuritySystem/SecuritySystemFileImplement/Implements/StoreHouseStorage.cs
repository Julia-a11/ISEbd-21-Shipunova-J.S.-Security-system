using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemFileImplement.Implements
{
    public class StoreHouseStorage : IStoreHouseStorage
    {
        private readonly FileDataListSingleton source;

        public StoreHouseStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<StoreHouseViewModel> GetFullList()
        {
            return source.StoreHouses
                .Select(CreateModel)
                .ToList();
        }

        public List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.StoreHouses
                .Where(rec => rec.StoreHouseName.Contains(model.StoreHouseName))
                .Select(CreateModel)
                .ToList();
        }

        public StoreHouseViewModel GetElement(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var storeHouse = source.StoreHouses.FirstOrDefault(rec => rec.StoreHouseName == model.StoreHouseName ||
            rec.Id == model.Id);
            return storeHouse != null ? CreateModel(storeHouse) : null;
        }

        public void Insert(StoreHouseBindingModel model)
        {
            int maxId = source.StoreHouses.Count > 0 ? source.StoreHouses.Max(rec => rec.Id) : 0;
            var storeHouse = new StoreHouse
            {
                Id = maxId + 1,
                StoreHouseComponents = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };
            source.StoreHouses.Add(CreateModel(model, storeHouse));
        }

        public void Update(StoreHouseBindingModel model)
        {
            var storeHouse = source.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }
            CreateModel(model, storeHouse);
        }

        public void Delete(StoreHouseBindingModel model)
        {
            var storeHouse = source.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (storeHouse == null)
            {
                throw new Exception("Склад не найден");
            }
            source.StoreHouses.Remove(storeHouse);
        }

        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storeHouse)
        {
            storeHouse.StoreHouseName = model.StoreHouseName;
            storeHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;

            foreach (var key in storeHouse.StoreHouseComponents.Keys.ToList())
            {
                if (!model.StoreHouseComponents.ContainsKey(key))
                {
                    storeHouse.StoreHouseComponents.Remove(key);
                }
            }

            foreach (var component in model.StoreHouseComponents)
            {
                if (storeHouse.StoreHouseComponents.ContainsKey(component.Key))
                {
                    storeHouse.StoreHouseComponents[component.Key] =
                    model.StoreHouseComponents[component.Key].Item2;
                }
                else
                {
                    storeHouse.StoreHouseComponents.Add(component.Key,
                    model.StoreHouseComponents[component.Key].Item2);
                }
            }
            return storeHouse;
        }

        private StoreHouseViewModel CreateModel(StoreHouse storeHouse)
        {
            Dictionary<int, (string, int)> storeHouseComponents = new Dictionary<int, (string, int)>();

            foreach (var storeHouseComponent in storeHouse.StoreHouseComponents)
            {
                string componentName = string.Empty;
                foreach (var component in source.Components)
                {
                    if (storeHouseComponent.Key == component.Id)
                    {
                        componentName = component.ComponentName;
                        break;
                    }
                }
                storeHouseComponents.Add(storeHouseComponent.Key, (componentName, storeHouseComponent.Value));
            }
            return new StoreHouseViewModel
            {
                Id = storeHouse.Id,
                StoreHouseName = storeHouse.StoreHouseName,
                ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                DateCreate = storeHouse.DateCreate,
                StoreHouseComponents = storeHouseComponents
            };
        }

        public bool CheckAndTake(int count, Dictionary<int, (string, int)> components)
        {
            foreach (var component in components)
            {
                int requiredCount = component.Value.Item2 * count;
                int availableCount = source.StoreHouses
                    .Where(rec => rec.StoreHouseComponents.ContainsKey(component.Key))
                    .Sum(rec => rec.StoreHouseComponents[component.Key]);
                if (availableCount < requiredCount)
                {
                    return false;
                }
            }
            foreach (var component in components)
            {
                int requiredCount = component.Value.Item2 * count;
                List<StoreHouse> availableStoreHouses = source.StoreHouses
                    .Where(rec => rec.StoreHouseComponents.ContainsKey(component.Key))
                    .ToList();
                foreach (var storeHouse in availableStoreHouses)
                {
                    int availableCount = storeHouse.StoreHouseComponents[component.Key];
                    if (availableCount <= requiredCount)
                    {
                        requiredCount = requiredCount - availableCount;
                        storeHouse.StoreHouseComponents.Remove(component.Key);
                    }
                    else
                    {
                        storeHouse.StoreHouseComponents[component.Key] -= requiredCount;
                        requiredCount = 0;
                    }
                    if (requiredCount == 0)
                    {
                        break;
                    }
                }
            }
            return true;
        }
    }
}