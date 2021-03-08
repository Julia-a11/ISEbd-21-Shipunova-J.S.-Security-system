using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.BindingModels
{
    public class StoreHouseBindingModel
    {
        public int? Id { get; set; }

        public string StoreHouseName { get; set; }

        public string ResponsiblePersonFCS { get; set; }

        public DateTime DateCreate { get; set; }

        public Dictionary<int, (string, int)> StoreHouseComponents { get; set; }
    }
}
