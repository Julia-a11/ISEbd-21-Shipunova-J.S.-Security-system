using Microsoft.EntityFrameworkCore;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemDatabaseImplement.Implements
{
    public class StoreHouseStorage : IStoreHouseStorage
    {

        private StoreHouse CreateModel(StoreHouseBindingModel model, StoreHouse storeHouse, SecuritySystemDatabase context)
        {
            storeHouse.StoreHouseName = model.StoreHouseName;
            storeHouse.ResponsiblePersonFCS = model.ResponsiblePersonFCS;
            if (storeHouse.Id == 0)
            {
                storeHouse.DateCreate = DateTime.Now;
                context.StoreHouses.Add(storeHouse);
                context.SaveChanges();
            }

            if (model.Id.HasValue)
            {
                var storeHouseComponents = context.StoreHouseComponents
                    .Where(rec => rec.StoreHouseId == model.Id.Value)
                    .ToList();

                context.StoreHouseComponents.RemoveRange(storeHouseComponents
                    .Where(rec => !model.StoreHouseComponents.ContainsKey(rec.ComponentId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateComponent in storeHouseComponents)
                {
                    updateComponent.Count = model.StoreHouseComponents[updateComponent.ComponentId].Item2;
                    model.StoreHouseComponents.Remove(updateComponent.ComponentId);
                }
                context.SaveChanges();
            }

            foreach (var storeHouseComponent in model.StoreHouseComponents)
            {
                context.StoreHouseComponents.Add(new StoreHouseComponent
                {
                    StoreHouseId = storeHouse.Id,
                    ComponentId = storeHouseComponent.Key,
                    Count = storeHouseComponent.Value.Item2
                });
                context.SaveChanges();
            }

            return storeHouse;
        }

        public bool CheckAndTake(int count, Dictionary<int, (string, int)> components)
        {
            using (var context = new SecuritySystemDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var component in components)
                        {
                            int requiredCount = component.Value.Item2 * count;
                            IEnumerable<StoreHouseComponent> storeHouseComponents = context.StoreHouseComponents
                                .Where(rec => rec.ComponentId == component.Key);

                            foreach (StoreHouseComponent storeHouseComponent in storeHouseComponents)
                            {
                                if (storeHouseComponent.Count <= requiredCount)
                                {
                                    requiredCount -= storeHouseComponent.Count;
                                    context.StoreHouseComponents.Remove(storeHouseComponent);
                                }
                                else
                                {
                                    storeHouseComponent.Count -= requiredCount;
                                    requiredCount = 0;
                                    break;
                                }
                            }

                            if (requiredCount != 0)
                            {
                                throw new Exception("Не хвататет компонентов на складе");
                            }
                        }

                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Delete(StoreHouseBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var storeHouse = context.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

                if (storeHouse == null)
                {
                    throw new Exception("Склад не найден");
                }

                context.StoreHouses.Remove(storeHouse);
                context.SaveChanges();
            }
        }

        public StoreHouseViewModel GetElement(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                var storeHouse = context.StoreHouses
                    .Include(rec => rec.StoreHouseComponents)
                    .ThenInclude(rec => rec.Component)
                    .FirstOrDefault(rec => rec.StoreHouseName == model.StoreHouseName ||
                    rec.Id == model.Id);

                return storeHouse != null ?
                    new StoreHouseViewModel
                    {
                        Id = storeHouse.Id,
                        StoreHouseName = storeHouse.StoreHouseName,
                        ResponsiblePersonFCS = storeHouse.ResponsiblePersonFCS,
                        DateCreate = storeHouse.DateCreate,
                        StoreHouseComponents = storeHouse.StoreHouseComponents
                            .ToDictionary(recStoreHouseComponent => recStoreHouseComponent.ComponentId,
                            recStoreHouseComponent => (recStoreHouseComponent.Component?.ComponentName,
                            recStoreHouseComponent.Count))
                    } :
                    null;
            }
        }

        public List<StoreHouseViewModel> GetFilteredList(StoreHouseBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                return context.StoreHouses
                    .Include(rec => rec.StoreHouseComponents)
                    .ThenInclude(rec => rec.Component)
                    .Where(rec => rec.StoreHouseName.Contains(model.StoreHouseName))
                    .ToList()
                    .Select(rec => new StoreHouseViewModel
                    {
                        Id = rec.Id,
                        StoreHouseName = rec.StoreHouseName,
                        ResponsiblePersonFCS = rec.ResponsiblePersonFCS,
                        DateCreate = rec.DateCreate,
                        StoreHouseComponents = rec.StoreHouseComponents
                            .ToDictionary(recStoreHouseComponent => recStoreHouseComponent.ComponentId,
                            recStoreHouseComponent => (recStoreHouseComponent.Component?.ComponentName,
                            recStoreHouseComponent.Count))
                    })
                    .ToList();
            }
        }

        public List<StoreHouseViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.StoreHouses.Count() == 0 ? new List<StoreHouseViewModel>() : 
                    context.StoreHouses
                    .Include(rec => rec.StoreHouseComponents)
                    .ThenInclude(rec => rec.Component)
                    .ToList()
                    .Select(rec => new StoreHouseViewModel
                    {
                        Id = rec.Id,
                        StoreHouseName = rec.StoreHouseName,
                        ResponsiblePersonFCS = rec.ResponsiblePersonFCS,
                        DateCreate = rec.DateCreate,
                        StoreHouseComponents = rec.StoreHouseComponents
                            .ToDictionary(recStoreHouseComponents => recStoreHouseComponents.ComponentId,
                            recStoreHouseComponents => (recStoreHouseComponents.Component?.ComponentName,
                            recStoreHouseComponents.Count))
                    })
                    .ToList();
            }
        }

        public void Insert(StoreHouseBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new StoreHouse(), context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(StoreHouseBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var storeHouse = context.StoreHouses.FirstOrDefault(rec => rec.Id == model.Id);

                        if (storeHouse == null)
                        {
                            throw new Exception("Склад не найден");
                        }

                        CreateModel(model, storeHouse, context);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
