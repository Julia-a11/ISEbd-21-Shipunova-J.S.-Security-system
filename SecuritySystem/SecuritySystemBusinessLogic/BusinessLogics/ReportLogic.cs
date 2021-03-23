using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.HelperModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IComponentStorage _componentStorage;

        private readonly ISecureStorage _secureStorage;

        private readonly IOrderStorage _orderStorage;

        public ReportLogic(ISecureStorage secureStorage, IComponentStorage componentStorage,
            IOrderStorage orderStorage)
        {
            _secureStorage = secureStorage;
            _componentStorage = componentStorage;
            _orderStorage = orderStorage;
        }

        //  Получение списка компонент с указанием, в каких изделиях используются
        public List<ReportSecureComponentViewModel> GetSecureComponent()
        {
            var components = _componentStorage.GetFullList();

            var secures = _secureStorage.GetFullList();

            var list = new List<ReportSecureComponentViewModel>();

            foreach (var component in components)
            {
                var record = new ReportSecureComponentViewModel
                {
                    ComponentName = component.ComponentName,
                    Secures = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var secure in secures)
                {
                    if (secure.SecureComponents.ContainsKey(component.Id))
                    {
                        record.Secures.Add(new Tuple<string, int>(secure.SecureName,
                            secure.SecureComponents[component.Id].Item2));
                        record.TotalCount += secure.SecureComponents[component.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }

        // Получение списка заказов за определенный период
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
                .Select(x => new ReportOrdersViewModel
                {
                    DateCreate = x.DateCreate,
                    SecureName = x.SecureName,
                    Count = x.Count,
                    Sum = x.Sum,
                    Status = x.Status

                })
                .ToList();
        }

        // Сохранение компонент в файл-Word
        public void SaveSecuresToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список комп",
                Secures = _secureStorage.GetFullList()
            });
        }

        // Сохранение компонент с указаеним продуктов в файл-Excel
        public void SaveSecureComponentToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонентов",
                SecureComponents = GetSecureComponent()
            });
        }

        // Сохранение заказов в файл-Pdf
        [Obsolete]
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
    }
}
