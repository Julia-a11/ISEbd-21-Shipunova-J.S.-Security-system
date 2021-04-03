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
    public class ClientStorage : IClientStorage
    {
        private Client CreateModel(ClientBindingModel model, Client client)
        {
            client.ClientFIO = model.ClientFIO;
            client.Login = model.Login;
            client.Password = model.Password;
            return client;
        }

        private ClientViewModel CreateModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                ClientFIO = client.ClientFIO,
                Login = client.Login,
                Password = client.Password
            };
        }

        public List<ClientViewModel> GetFullList()
        {
            using (var context = new SecuritySystemDatabase())
            {
                return context.Clients
                    .Select(CreateModel).ToList();
            }
        }

        public List<ClientViewModel> GetFilteredList(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                return context.Clients
                    .Where(rec => rec.ClientFIO.Contains(model.ClientFIO) || (rec.Login.Equals(model.Login) &&
                    rec.Password.Equals(model.Password)))
                    .Select(CreateModel)
                    .ToList();
            }
        }

        public ClientViewModel GetElement(ClientBindingModel model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new SecuritySystemDatabase())
            {
                var client = context.Clients
                    .FirstOrDefault(rec => rec.Login.Equals(model.Login) ||
                    rec.Id == model.Id);

                return client != null ?
                    CreateModel(client) :  null;
            }
        }

        public void Insert(ClientBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                context.Clients.Add(CreateModel(model, new Client()));
                context.SaveChanges();
            }
        }

        public void Update(ClientBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var client = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);

                if (client == null)
                {
                    throw new Exception("Клиент не найден");
                }

                CreateModel(model, client);
                context.SaveChanges();
            }
        }

        public void Delete(ClientBindingModel model)
        {
            using (var context = new SecuritySystemDatabase())
            {
                var client = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);

                if (client == null)
                {
                    throw new Exception("Клиент не найден");
                }

                context.Clients.Remove(client);
                context.SaveChanges();
            }
        }
    }
}
