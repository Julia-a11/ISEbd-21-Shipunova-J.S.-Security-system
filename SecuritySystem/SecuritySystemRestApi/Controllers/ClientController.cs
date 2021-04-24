using Microsoft.AspNetCore.Mvc;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuritySystemRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientLogic _logic;

        public ClientController(ClientLogic logic)
        {
            _logic = logic;
        }

        [HttpGet]
        public ClientViewModel Login(string login, string password)
        {
            return _logic.Read(new ClientBindingModel
            {
                Email = login,
                Password = password
            }).FirstOrDefault();
        }

        [HttpPost]
        public void Register(ClientBindingModel model) => _logic.CreateOrUpdate(model);
       
        [HttpPost]
        public void UpdateData(ClientBindingModel model) => _logic.CreateOrUpdate(model);

    }
}
