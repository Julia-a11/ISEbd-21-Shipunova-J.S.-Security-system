using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemListImplement.Implements
{
    public class StoreHouseStorage : IStoreHouseStorage
    {
        private readonly DataListSingleton source;

        public StoreHouseStorage()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<StoreHouseViewModel> GetFullList()
        {
            List<StoreHouseViewModel> result = new List<StoreHouseViewModel>();
            foreach (var storeHouse in source.Storehouses)
            {
                result.Add(CreateModel(storeHouse));
            }
            return result;
        }

        public List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            List<StoreHouseViewModel> result = new List<StoreHouseViewModel>();
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.StoreHouseName.Contains(model.StoreHouseName))
                {
                    result.Add(CreateModel(storeHouse));
                }
            }
            return result;
        }

        public StoreHouseViewModel GetElement(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.Id == model.Id || storeHouse.StoreHouseName ==
                model.StoreHouseName)
                {
                    return CreateModel(storeHouse);
                }
            }
            return null;
        }

        public void Insert(StoreHouseBindingModel model)
        {
            StoreHouse tempStoreHouse = new StoreHouse
            {
                Id = 1,
                StoreHouseComponents = new Dictionary<int, int>(),
                DateCreate = DateTime.Now
            };
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.Id >= tempStoreHouse.Id)
                {
                    tempStoreHouse.Id = storeHouse.Id + 1;
                }
            }
            source.Storehouses.Add(CreateModel(model, tempStoreHouse));
        }

        public void Update(StoreHouseBindingModel model)
        {
            StoreHouse tempStoreHouse = null;
            foreach (var storeHouse in source.Storehouses)
            {
                if (storeHouse.Id == model.Id)
                {
                    tempStoreHouse = storeHouse;
                }
            }
            if (tempStoreHouse == null)
            {
                throw new Exception("Склад не найден");
            }
            CreateModel(model, tempStoreHouse);
        }

        public void Delete(StoreHouseBindingModel model)
        {
            for (int i = 0; i < source.Storehouses.Count; ++i)
            {
                if (source.Storehouses[i].Id == model.Id)
                {
                    source.Storehouses.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Склад не найден");
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

        public void Print()
        {
            foreach (StoreHouse storehouse in source.Storehouses)
            {
                Console.WriteLine(storehouse.StoreHouseName + " " + storehouse.ResponsiblePersonFCS + " " + storehouse.DateCreate);
                foreach (KeyValuePair<int, int> keyValue in storehouse.StoreHouseComponents)
                {
                    string componentName = source.Components.FirstOrDefault(component => component.Id == keyValue.Key).ComponentName;
                    Console.WriteLine(componentName + " " + keyValue.Value);
                }
            }
        }
    }
}