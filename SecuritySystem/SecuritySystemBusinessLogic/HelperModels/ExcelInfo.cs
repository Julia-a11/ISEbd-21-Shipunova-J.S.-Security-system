using SecuritySystemBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace SecuritySystemBusinessLogic.HelperModels
{
    public class ExcelInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportSecureComponentViewModel> SecureComponents { get; set; }
    }
}