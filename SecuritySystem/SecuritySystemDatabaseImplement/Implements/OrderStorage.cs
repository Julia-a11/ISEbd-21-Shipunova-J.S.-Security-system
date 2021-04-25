﻿using Microsoft.EntityFrameworkCore;
using SecuritySystemBusinessLogic.BindingModels;
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
            order.Sum = model.Sum;
            order.Count = model.Count;
            order.Status = model.Status;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;

            return order;
        }

        public List<OrderViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.Orders
                    .Include(rec => rec.Secure)
                    .Select(rec => new OrderViewModel
                    {
                        Id = rec.Id,
                        SecureName = rec.Secure.SecureName,
                        SecureId = rec.SecureId,
                        Count = rec.Count,
                        Sum = rec.Sum,
                        Status = rec.Status,
                        DateCreate = rec.DateCreate,
                        DateImplement = rec.DateImplement
                    })
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
                    .Where(rec => (rec.SecureId == model.SecureId) ||
                        (!model.DateFrom.HasValue && !model.DateTo.HasValue && 
                        rec.DateCreate.Date == model.DateCreate.Date) ||
                        (model.DateFrom.HasValue && model.DateTo.HasValue && 
                        rec.DateCreate.Date >= model.DateFrom.Value.Date && rec.DateCreate.Date <= model.DateTo.Value.Date))
                    .Select(rec => new OrderViewModel
                    {
                        Id = rec.Id,
                        SecureName = rec.Secure.SecureName,
                        SecureId = rec.SecureId,
                        Count = rec.Count,
                        Sum = rec.Sum,
                        Status = rec.Status,
                        DateCreate = rec.DateCreate,
                        DateImplement = rec.DateImplement
                    })
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
                    .FirstOrDefault(rec => rec.Id == model.Id);

                return order != null ?
                    new OrderViewModel
                    {
                        Id = order.Id,
                        SecureName = order.Secure.SecureName,
                        SecureId = order.SecureId,
                        Count = order.Count,
                        Sum = order.Sum,
                        Status = order.Status,
                        DateCreate = order.DateCreate,
                        DateImplement = order.DateImplement
                    } :
                    null;
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
