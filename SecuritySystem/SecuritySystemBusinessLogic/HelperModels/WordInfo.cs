using SecuritySystemBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace SecuritySystemBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<SecureViewModel> Secures { get; set; }
    }
}