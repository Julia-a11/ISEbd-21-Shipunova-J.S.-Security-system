using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public class WorkModeling
    {
        private readonly IImplementerStorage _implementerStorage;

        private readonly IOrderStorage _orderStorage;

        private readonly OrderLogic _orderLogic;

        private readonly Random rnd;

        public WorkModeling(IImplementerStorage implementerStorage, IOrderStorage orderStorage, OrderLogic orderLogic)
        {
            _implementerStorage = implementerStorage;
            _orderStorage = orderStorage;
            _orderLogic = orderLogic;

            rnd = new Random(1000);
        }

        // Запуск работ
        public void DoWork()
        {
            var implementers = _implementerStorage.GetFullList();

            var orders = _orderStorage.GetFilteredList(new OrderBindingModel { FreeOrders = true });

            foreach (var implementer in implementers)
            {
                WorkerWorkAsync(implementer, orders);
            }
        }

        // Имитация работы исполнителя
        private async void WorkerWorkAsync(ImplementerViewModel implementer, 
            List<OrderViewModel> orders)
        {
            // Ищем заказы, которые уже в работе (вдруг исполнителя прервали)
            var runOrders = await Task.Run(() => _orderStorage.GetFilteredList(new OrderBindingModel
            {
                ImplementerId = implementer.Id
            }));

            foreach (var order in runOrders)
            {
                // Делаем работу заново
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);

                _orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id
                });

                // Отдыхаем
                Thread.Sleep(implementer.PauseTime);
            }

            var ordersRequiringMaterials = await Task.Run(() => _orderStorage.GetFullList().Where(rec => rec.Status == Enums.OrderStatus.ТребуютсяMатериалы).ToList());
            foreach (var order in ordersRequiringMaterials)
            {
                try
                {
                    _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id,
                        ImplementerId = implementer.Id
                    });
                    Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                    _orderLogic.FinishOrder(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id
                    });
                    Thread.Sleep(implementer.PauseTime);
                }
                catch (Exception) { }
            }

            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    // Пытаемся назначить заказ на исполнителя
                    try
                    {
                        _orderLogic.TakeOrderInWork(new ChangeStatusBindingModel
                        {
                            OrderId = order.Id,
                            ImplementerId = implementer.Id
                        });
                        // Делаем работу
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        _orderLogic.FinishOrder(new ChangeStatusBindingModel
                        {
                            OrderId = order.Id
                        });
                        // Отдыхаем
                        Thread.Sleep(implementer.PauseTime);
                    }
                    catch (Exception) { }
                }
            });
        }
    }
}