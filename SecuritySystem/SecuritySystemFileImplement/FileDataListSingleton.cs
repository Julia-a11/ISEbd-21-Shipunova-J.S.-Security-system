﻿using SecuritySystemBusinessLogic.Enums;
using SecuritySystemFileImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace SecuritySystemFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;

        private readonly string ComponentFileName = "Component.xml";

        private readonly string OrderFileName = "Order.xml";

        private readonly string SecureFileName = "Secure.xml";

        public List<Component> Components { get; set; }

        public List<Order> Orders { get; set; }

        public List<Secure> Secures { get; set; }

        private FileDataListSingleton()
        {
            Components = LoadComponents();
            Orders = LoadOrders();
            Secures = LoadSecures();
        }

        public static FileDataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new FileDataListSingleton();
            }
            return instance;
        }

        ~FileDataListSingleton()
        {
            SaveComponents();
            SaveOrders();
            SaveSecures();
        }

        private List<Component> LoadComponents()
        {
            var list = new List<Component>();

            if (File.Exists(ComponentFileName))
            {
                XDocument xDocument = XDocument.Load(ComponentFileName);

                var xElements = xDocument.Root.Elements("Component").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Component
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ComponentName = elem.Element("ComponentName").Value
                    });
                }
            }
            return list;
        }

        private List<Order> LoadOrders()
        {
            var list = new List<Order>();

            if (File.Exists(OrderFileName))
            {
                XDocument xDocument = XDocument.Load(OrderFileName);

                var xElements = xDocument.Root.Elements("Order").ToList();

                foreach (var elem in xElements)
                {
                    list.Add(new Order
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SecureId = Convert.ToInt32(elem.Element("SecureId").Value),
                        Count = Convert.ToInt32(elem.Element("Count").Value),
                        Sum = Convert.ToDecimal(elem.Element("Sum").Value),
                        Status = (OrderStatus)Convert.ToInt32(elem.Element("Status").Value),
                        DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value),
                        DateImplement = (!string.IsNullOrEmpty(elem.Element("DateImplement").Value)) ? Convert.ToDateTime(elem.Element("DateImplement").Value) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        private List<Secure> LoadSecures()
        {
            var list = new List<Secure>();

            if (File.Exists(SecureFileName))
            {
                XDocument xDocument = XDocument.Load(SecureFileName);

                var xElements = xDocument.Root.Elements("Secure").ToList();

                foreach (var elem in xElements)
                {
                    var prodComp = new Dictionary<int, int>();
                    foreach (var component in elem.Element("SecureComponents").Elements("SecureComponent").ToList())
                    {
                        prodComp.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new Secure
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SecureName = elem.Element("SecureName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value),
                        SecureComponents = prodComp
                    });
                }
            }
            return list;
        }
        private void SaveComponents()
        {
            if (Components != null)
            {
                var xElement = new XElement("Components");

                foreach (var component in Components)
                {
                    xElement.Add(new XElement("Component",
                        new XAttribute("Id", component.Id),
                        new XElement("ComponentName", component.ComponentName)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ComponentFileName);
            }
        }

        private void SaveOrders()
        {
            if (Orders != null)
            {
                var xElement = new XElement("Orders");

                foreach (var order in Orders)
                {
                    xElement.Add(new XElement("Order",
                        new XAttribute("Id", order.Id),
                        new XElement("SecureId", order.SecureId),
                        new XElement("Count", order.Count),
                        new XElement("Sum", order.Sum),
                        new XElement("Status", (int)order.Status),
                        new XElement("DateCreate", order.DateCreate),
                        new XElement("DateImplement", order.DateImplement)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(OrderFileName);
            }
        }

        private void SaveSecures()
        {
            if (Secures != null)
            {
                var xElement = new XElement("Secures");

                foreach (var Secure in Secures)
                {
                    var compElement = new XElement("SecureComponents");
                    foreach (var component in Secure.SecureComponents)
                    {
                        compElement.Add(new XElement("SecureComponent",
                            new XElement("Key", component.Key),
                            new XElement("Value", component.Value)));
                    }
                    xElement.Add(new XElement("Secure",
                        new XAttribute("Id", Secure.Id),
                        new XElement("SecureName", Secure.SecureName),
                        new XElement("Price", Secure.Price),
                        compElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(SecureFileName);
            }
        }
    }
}
