using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.HelperModels
{
    public class WordInfoForStoreHouse
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<StoreHouseViewModel> StoreHouses { get; set; }
    }
}
