using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemFileImplement.Implements
{
    public class ComponentStorage : IComponentStorage
    {
        private readonly FileDataListSingleton source;

        public ComponentStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<ComponentViewModel> GetFullList()
        {
            return source.Components
                .Select(CreateModel)
                .ToList();
        }

        public List<ComponentViewModel> GetFilteredList(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            return source.Components
                .Where(rec => rec.ComponentName.Contains(model.ComponentName))
                .Select(CreateModel)
                .ToList();
        }

        public ComponentViewModel GetElement(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            var component = source.Components
                .FirstOrDefault(rec => rec.ComponentName == model.ComponentName ||
                rec.Id == model.Id);
            return component != null ? CreateModel(component) : null;
        }

        public void Insert(ComponentBindingModel model)
        {
            int maxId = source.Components.Count > 0 ? source.Components.Max(
                rec => rec.Id) : 0;
            var component = new Component { Id = maxId + 1 };
            source.Components.Add(CreateModel(model, component));
        }

        public void Update(ComponentBindingModel model)
        {
            var component = source.Components.FirstOrDefault(rec => rec.Id == model.Id);
            if (component == null)
            {
                throw new Exception("Компонент не найден");
            }

            CreateModel(model, component);
        }

        public void Delete(ComponentBindingModel model)
        {
            Component component = source.Components.FirstOrDefault(rec => rec.Id == model.Id);
            if (component != null)
            {
                source.Components.Remove(component);
            }
            else
            {
                throw new Exception("Компонент не найден");
            }
        }

        private Component CreateModel(ComponentBindingModel model, Component component)
        {
            component.ComponentName = model.ComponentName;

            return component;
        }

        private ComponentViewModel CreateModel(Component component)
        {
            return new ComponentViewModel
            {
                Id = component.Id,
                ComponentName = component.ComponentName
            };
        }
    }
}