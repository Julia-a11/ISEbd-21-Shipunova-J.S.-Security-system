using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemFileImplement.Implements
{
    public class SecureStorage : ISecureStorage
    {
        private readonly FileDataListSingleton source;

        public SecureStorage()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public List<SecureViewModel> GetFullList()
        {
            return source.Secures
                .Select(CreateModel)
                .ToList();
        }

        public List<SecureViewModel> GetFilteredList(SecureBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return source.Secures
                .Where(rec => rec.SecureName.Contains(model.SecureName))
                .Select(CreateModel)
                .ToList();
        }

        public SecureViewModel GetElement(SecureBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            var secure = source.Secures
                .FirstOrDefault(rec => rec.SecureName == model.SecureName || rec.Id == model.Id);
            return secure != null ? CreateModel(secure) : null;
        }

        public void Insert(SecureBindingModel model)
        {
            int maxId = source.Secures.Count > 0 ? source.Components.Max(rec => rec.Id) : 0;

            var element = new Secure
            {
                Id = maxId + 1,
                SecureComponents = new
                Dictionary<int, int>()
            };
            source.Secures.Add(CreateModel(model, element));
        }

        public void Update(SecureBindingModel model)
        {
            var element = source.Secures.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
        }

        public void Delete(SecureBindingModel model)
        {
            Secure element = source.Secures.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Secures.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        private Secure CreateModel(SecureBindingModel model, Secure secure)
        {
            secure.SecureName = model.SecureName;
            secure.Price = model.Price;
            // удаляем убранные
            foreach (var key in secure.SecureComponents.Keys.ToList())
            {
                if (!model.SecureComponents.ContainsKey(key))
                {
                    secure.SecureComponents.Remove(key);
                }
            }
            // обновляем существующий и добавляем новые
            foreach (var component in model.SecureComponents)
            {
                if (secure.SecureComponents.ContainsKey(component.Key))
                {
                    secure.SecureComponents[component.Key] = model.SecureComponents[component.Key].Item2;
                }
                else
                {
                    secure.SecureComponents.Add(component.Key, model.SecureComponents[component.Key].Item2);
                }
            }
            return secure;
        }

        private SecureViewModel CreateModel(Secure secure)
        {
            return new SecureViewModel
            {
                Id = secure.Id,
                SecureName = secure.SecureName,
                Price = secure.Price,
                SecureComponents = secure.SecureComponents
                                .ToDictionary(recPc => recPc.Key, recPc =>
                                (source.Components.FirstOrDefault(recC => recC.Id == recPc.Key)?.ComponentName, recPc.Value))
            };
        }
    }
}
