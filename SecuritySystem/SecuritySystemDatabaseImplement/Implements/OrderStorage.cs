using Microsoft.EntityFrameworkCore;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Enums;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using SecuritySystemDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private Order CreateModel(OrderBindingModel model, Order order)
        {
            order.SecureId = model.SecureId;
            order.ClientId = model.ClientId.Value;
            order.ImplementerId = model.ImplementerId;
            order.Sum = model.Sum;
            order.Count = model.Count;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }

        private OrderViewModel CreateModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                SecureId = order.SecureId,
                ClientId = order.ClientId,
                ImplementerId = order.ImplementerId,
                ClientFIO = order.Client.ClientFIO,
                ImplementerFIO = order.ImplementerId.HasValue ? order.Implementer.ImplementerFIO : string.Empty,
                SecureName = order.Secure.SecureName,
                Sum = order.Sum,
                Count = order.Count,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement
            };
        }

        public List<OrderViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.Orders
                    .Include(rec => rec.Secure)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public List<OrderViewModel> GetFilteredList(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                return context.Orders
                    .Include(rec => rec.Secure)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .Where(rec => (rec.SecureId == model.SecureId) || (!model.DateFrom.HasValue && !model.DateTo.HasValue &&
                    rec.DateCreate.Date == model.DateCreate.Date) ||
                    (model.DateFrom.HasValue && model.DateTo.HasValue &&
                    rec.DateCreate.Date >= model.DateFrom.Value.Date &&
                    rec.DateCreate.Date <= model.DateTo.Value.Date) ||
                    (model.ClientId.HasValue && rec.ClientId == model.ClientId) ||
                    (model.FreeOrders.HasValue && model.FreeOrders.Value && rec.Status == OrderStatus.Принят) ||
                    (model.ImplementerId.HasValue && rec.ImplementerId == model.ImplementerId &&
                    rec.Status == OrderStatus.Выполняется))
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public OrderViewModel GetElement(OrderBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                var order = context.Orders
                    .Include(rec => rec.Secure)
                    .Include(rec => rec.Client)
                    .Include(rec => rec.Implementer)
                    .FirstOrDefault(rec => rec.Id == model.Id);

                return order != null ? CreateModel(order) : null;
            }
        }

        public void Insert(OrderBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                context.Orders.Add(CreateModel(model, new Order()));
                context.SaveChanges();
            }
        }

        public void Update(OrderBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var order = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);

                if (order == null)
                {
                    throw new Exception("Заказ не найден");
                }

                CreateModel(model, order);
                context.SaveChanges();
            }
        }

        public void Delete(OrderBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var order = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);

                if (order == null)
                {
                    throw new Exception("Заказ не найден");
                }

                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }
    }
}
