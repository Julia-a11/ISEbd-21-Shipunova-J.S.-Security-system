using SecuritySystemListImplement.Models;
using System.Collections.Generic;

namespace SecuritySystemListImplement
{
    public class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Component> Components { get; set; }

        public List<Order> Orders { get; set; }

        public List<Secure> Secures { get; set; }

        public List<Client> Clients { get; set; }

        private DataListSingleton()
        {
            Components = new List<Component>();
            Orders = new List<Order>();
            Secures = new List<Secure>();
            Clients = new List<Client>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }
    }
}