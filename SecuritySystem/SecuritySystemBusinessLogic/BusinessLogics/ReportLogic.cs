using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.HelperModels;
using SecuritySystemBusinessLogic.Interfaces;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly ISecureStorage _secureStorage;

        private readonly IOrderStorage _orderStorage;

        public ReportLogic(ISecureStorage secureStorage, IOrderStorage orderStorage)
        {
            _secureStorage = secureStorage;
            _orderStorage = orderStorage;
        }

        //  Получение списка компонент с указанием, в каких изделиях используются
        public List<ReportSecureComponentViewModel> GetSecureComponent()
        {
            var secures = _secureStorage.GetFullList();

            var list = new List<ReportSecureComponentViewModel>();

            foreach (var secure in secures)
            {
                var record = new ReportSecureComponentViewModel
                {
                    SecureName = secure.SecureName,
                    Components = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var component in secure.SecureComponents)
                {
                    record.Components.Add(new Tuple<string, int>(component.Value.Item1, component.Value.Item2));
                    record.TotalCount += component.Value.Item2;
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
                Title = "Список комплектаций",
                Secures = _secureStorage.GetFullList()
            });
        }

        // Сохранение компонент с указаеним продуктов в файл-Excel
        public void SaveSecureComponentToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список комплектаций",
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
