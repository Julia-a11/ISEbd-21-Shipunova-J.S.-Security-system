using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemDatabaseImplement.Implements
{
    public class ComponentStorage : IComponentStorage
    {
        private Component CreateModel(ComponentBindingModel model, Component component)
        {
            component.ComponentName = model.ComponentName;
            return component;
        }

        public List<ComponentViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.Components
                    .Select(rec => new ComponentViewModel
                    {
                        Id = rec.Id,
                        ComponentName = rec.ComponentName
                    })
                    .ToList();
            }
        }

        public List<ComponentViewModel> GetFilteredList(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                return context.Components
                    .Where(rec => rec.ComponentName.Contains(model.ComponentName))
                    .Select(rec => new ComponentViewModel
                    {
                        Id = rec.Id,
                        ComponentName = rec.ComponentName
                    })
                    .ToList();
            }
        }

        public ComponentViewModel GetElement(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                var component = context.Components
                    .FirstOrDefault(rec => rec.ComponentName == model.ComponentName ||
                    rec.Id == model.Id);

                return component != null ?
                    new ComponentViewModel
                    {
                        Id = component.Id,
                        ComponentName = component.ComponentName
                    } :
                    null;
            }
        }

        public void Insert(ComponentBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                context.Components.Add(CreateModel(model, new Component()));
                context.SaveChanges();
            }
        }

        public void Update(ComponentBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var component = context.Components.FirstOrDefault(rec => rec.Id == model.Id);

                if (component == null)
                {
                    throw new Exception("Компонент не найден");
                }

                CreateModel(model, component);
                context.SaveChanges();
            }
        }

        public void Delete(ComponentBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var component = context.Components.FirstOrDefault(rec => rec.Id == model.Id);

                if (component == null)
                {
                    throw new Exception("Компонент не найден");
                }

                context.Components.Remove(component);
                context.SaveChanges();
            }
        }
    }
}
