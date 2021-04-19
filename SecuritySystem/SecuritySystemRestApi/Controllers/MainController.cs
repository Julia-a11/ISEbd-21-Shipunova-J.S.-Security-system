using Microsoft.AspNetCore.Mvc;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SecuritySystemRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class MainController : ControllerBase
    {
        private readonly OrderLogic _order;

        private readonly SecureLogic _secure;

        private readonly OrderLogic _main;

        public MainController(OrderLogic order, SecureLogic secure, OrderLogic main)
        {
            _order = order;
            _secure = secure;
            _main = main;
        }

        [HttpGet]
        public List<SecureViewModel> GetSecureList() => _secure.Read(null)?.ToList();

        [HttpGet]
        public SecureViewModel GetSecure(int SecureId) => _secure.Read(new SecureBindingModel
        { Id = SecureId })?[0];

        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => _order.Read(new OrderBindingModel
        { ClientId = clientId });

        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) =>
       _main.CreateOrder(model);
    }
}
