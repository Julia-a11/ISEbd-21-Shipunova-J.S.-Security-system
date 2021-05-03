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
    public class StoreHouseController : Controller
    {
        private readonly StoreHouseLogic _storeHouseLogic;

        private readonly ComponentLogic _componentLogic;

        public StoreHouseController(StoreHouseLogic storeHouseLogic, ComponentLogic componentLogic)
        {
            _storeHouseLogic = storeHouseLogic;
            _componentLogic = componentLogic;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public List<StoreHouseViewModel> GetStoreHouses() => _storeHouseLogic.Read(null);

        [HttpPost]
        public void CreateStoreHouse(StoreHouseBindingModel model) => _storeHouseLogic.CreateOrUpdate(model);

        [HttpPost]
        public void UpdateStoreHouse(StoreHouseBindingModel model) => _storeHouseLogic.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteStoreHouse(StoreHouseBindingModel model) => _storeHouseLogic.Delete(model);

        [HttpPost]
        public void Replenishment(StoreHouseReplenishmentBindingModel model) => _storeHouseLogic.Replenishment(model);

        [HttpGet]
        public List<ComponentViewModel> GetComponents() => _componentLogic.Read(null);
    }
}
