using Microsoft.EntityFrameworkCore;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuritySystemDatabaseImplement.Implements
{
    public class SecureStorage : ISecureStorage
    {
        private Secure CreateModel(SecureBindingModel model, Secure secure, SecuritySystemDatabase context)
        {
            secure.SecureName = model.SecureName;
            secure.Price = model.Price;
            context.Secures.Add(secure);
            context.SaveChanges();

            if (model.Id.HasValue)
            {
                var SecureComponents = context.SecureComponents
                    .Where(rec => rec.SecureId == model.Id.Value)
                    .ToList();

                context.SecureComponents.RemoveRange(SecureComponents
                    .Where(rec => !model.SecureComponents.ContainsKey(rec.ComponentId))
                    .ToList());
                context.SaveChanges();

                foreach (var updateComponent in SecureComponents)
                {
                    updateComponent.Count = model.SecureComponents[updateComponent.ComponentId].Item2;
                    model.SecureComponents.Remove(updateComponent.ComponentId);
                }
                context.SaveChanges();
            }


            foreach (var SecureComponent in model.SecureComponents)
            {
                context.SecureComponents.Add(new SecureComponent
                {
                    SecureId = secure.Id,
                    ComponentId = SecureComponent.Key,
                    Count = SecureComponent.Value.Item2
                });
                context.SaveChanges();
            }

            return secure;
        }

        public List<SecureViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.Secures
                    .Include(rec => rec.SecureComponents)
                    .ThenInclude(rec => rec.Component)
                    .ToList()
                    .Select(rec => new SecureViewModel
                    {
                        Id = rec.Id,
                        SecureName = rec.SecureName,
                        Price = rec.Price,
                        SecureComponents = rec.SecureComponents
                            .ToDictionary(recSecureComponents => recSecureComponents.ComponentId,
                            recSecureComponents => (recSecureComponents.Component?.ComponentName,
                            recSecureComponents.Count))
                    })
                    .ToList();
            }
        }

        public List<SecureViewModel> GetFilteredList(SecureBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                return context.Secures
                    .Include(rec => rec.SecureComponents)
                    .ThenInclude(rec => rec.Component)
                    .Where(rec => rec.SecureName.Contains(model.SecureName))
                    .ToList()
                    .Select(rec => new SecureViewModel
                    {
                        Id = rec.Id,
                        SecureName = rec.SecureName,
                        Price = rec.Price,
                        SecureComponents = rec.SecureComponents
                            .ToDictionary(recSecureComponent => recSecureComponent.ComponentId,
                            recSecureComponent => (recSecureComponent.Component?.ComponentName,
                            recSecureComponent.Count))
                    })
                    .ToList();
            }
        }

        public SecureViewModel GetElement(SecureBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                var Secure = context.Secures
                    .Include(rec => rec.SecureComponents)
                    .ThenInclude(rec => rec.Component)
                    .FirstOrDefault(rec => rec.SecureName == model.SecureName ||
                    rec.Id == model.Id);

                return Secure != null ?
                    new SecureViewModel
                    {
                        Id = Secure.Id,
                        SecureName = Secure.SecureName,
                        Price = Secure.Price,
                        SecureComponents = Secure.SecureComponents
                            .ToDictionary(recSecureComponent => recSecureComponent.ComponentId,
                            recSecureComponent => (recSecureComponent.Component?.ComponentName,
                            recSecureComponent.Count))
                    } :
                    null;
            }
        }

        public void Insert(SecureBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        CreateModel(model, new Secure(), context);
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

        public void Update(SecureBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var Secure = context.Secures.FirstOrDefault(rec => rec.Id == model.Id);

                        if (Secure == null)
                        {
                            throw new Exception("Пицца не найдена");
                        }

                        context.Secures.Add(CreateModel(model, new Secure(), context));
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

        public void Delete(SecureBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var Secure = context.Secures.FirstOrDefault(rec => rec.Id == model.Id);

                if (Secure == null)
                {
                    throw new Exception("Компонент не найден");
                }

                context.Secures.Remove(Secure);
                context.SaveChanges();
            }
        }
    }
}
