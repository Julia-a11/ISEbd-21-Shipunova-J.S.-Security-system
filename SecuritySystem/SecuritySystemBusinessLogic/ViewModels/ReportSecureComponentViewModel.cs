using System;
using System.Collections.Generic;
using System.Text;

namespace SecuritySystemBusinessLogic.ViewModels
{
    public class ReportSecureComponentViewModel
    {
        public string ComponentName { get; set; }

        public int TotalCount { get; set; }

        public List<Tuple<string, int>> Secures { get; set; }
    }
}
