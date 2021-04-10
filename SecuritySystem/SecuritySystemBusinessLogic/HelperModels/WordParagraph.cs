using System.Collections.Generic;

namespace SecuritySystemBusinessLogic.HelperModels
{
    public class WordParagraph
    {
        public List<(string, WordParagraphProperties)> Texts { get; set; }

        public WordParagraphProperties TextProperties { get; set; }
    }
}