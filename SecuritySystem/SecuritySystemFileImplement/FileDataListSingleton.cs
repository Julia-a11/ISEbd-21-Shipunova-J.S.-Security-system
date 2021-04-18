using SecuritySystemBusinessLogic.Enums;
using SecuritySystemFileImplement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SecuritySystemFileImplement
{
    public class FileDataListSingleton
    {
        private static FileDataListSingleton instance;

        private readonly string ComponentFileName = "Component.xml";

        private readonly string OrderFileName = "Order.xml";

        private readonly string SecureFileName = "Secure.xml";

        private readonly string ClientFileName = "Client.xml";
        private readonly string StoreHouseFileName = "StoreHouse.xml";

        public List<Component> Components { get; set; }

        public List<Order> Orders { get; set; }

        public List<Secure> Secures { get; set; }

        public List<Client> Clients { get; set; }
        public List<StoreHouse> StoreHouses { get; set; }

        private FileDataListSingleton()
        {
            Components = LoadComponents();
            Orders = LoadOrders();
            Secures = LoadSecures();
            Clients = LoadClients();
            StoreHouses = LoadStoreHouses();
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
            SaveClients();
            SaveStoreHouses();
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
                        DateImplement = !string.IsNullOrEmpty(elem.Element("DateImplement").Value) ?
                            Convert.ToDateTime(elem.Element("DateImplement").Value) : DateTime.MinValue
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
                    var secureComponents = new Dictionary<int, int>();
                    foreach (var component in elem.Element("SecureComponents").Elements("SecureComponent").ToList())
                    {
                        secureComponents.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new Secure
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        SecureName = elem.Element("SecureName").Value,
                        Price = Convert.ToDecimal(elem.Element("Price").Value),
                        SecureComponents = secureComponents
                    });
                }
            }
            return list;
        }

        private List<StoreHouse> LoadStoreHouses()
        {
            var list = new List<StoreHouse>();

            if (File.Exists(StoreHouseFileName))
            {
                XDocument xDocument = XDocument.Load(StoreHouseFileName);

                var xElements = xDocument.Root.Elements("StoreHouse").ToList();

                foreach (var elem in xElements)
                {
                    var storeHouseComponents = new Dictionary<int, int>();
                    foreach (var component in elem.Element("StoreHouseComponents").Elements("StoreHouseComponent").ToList())
                    {
                        storeHouseComponents.Add(Convert.ToInt32(component.Element("Key").Value),
                            Convert.ToInt32(component.Element("Value").Value));
                    }
                    list.Add(new StoreHouse
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        StoreHouseName = elem.Element("StoreHouseName").Value,
                        ResponsiblePersonFCS = elem.Element("ResponsiblePersonFCS").Value,
                        StoreHouseComponents = storeHouseComponents,
                        DateCreate = Convert.ToDateTime(elem.Element("DateCreate").Value)
                        });
                    }
                }
                return list;
            }
        private List<Client> LoadClients()
        {
            var list = new List<Client>();
            if (File.Exists(ClientFileName))
            {
                XDocument xDocument = XDocument.Load(ClientFileName);
                var xElements = xDocument.Root.Elements("Clients").ToList();
                foreach (var elem in xElements)
                {
                    list.Add(new Client
                    {
                        Id = Convert.ToInt32(elem.Attribute("Id").Value),
                        ClientFIO = elem.Element("ClietnFIO").Value,
                        Login = elem.Element("Login").Value,
                        Password = elem.Element("Password").Value

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

        private void SaveStoreHouses()
        {
            if (StoreHouses != null)
            {
                var xElement = new XElement("StoreHouses");

                foreach (var storeHouse in StoreHouses)
                {
                    var compElement = new XElement("StoreHouseComponents");
                    foreach (var component in storeHouse.StoreHouseComponents)
                    {
                        compElement.Add(new XElement("StoreHouseComponent",
                            new XElement("Key", component.Key),
                            new XElement("Value", component.Value)));
                    }
                    xElement.Add(new XElement("StoreHouse",
                        new XAttribute("Id", storeHouse.Id),
                        new XElement("StoreHouseName", storeHouse.StoreHouseName),
                        new XElement("ResponsiblePersonFCS", storeHouse.ResponsiblePersonFCS),
                        new XElement("DateCreate", storeHouse.DateCreate),
                        compElement));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(StoreHouseFileName);
              }
            }
        private void SaveClients()
        {
            if (Clients != null)
            {
                var xElement = new XElement("Clients");
                foreach (var client in Clients)
                {
                    xElement.Add(new XElement("Client",
                        new XAttribute("Id", client.Id),
                        new XElement("ClientFIO", client.ClientFIO),
                        new XElement("Login", client.Login),
                        new XElement("Password", client.Password)));
                }
                XDocument xDocument = new XDocument(xElement);
                xDocument.Save(ClientFileName);
            }
        }
    }
}
