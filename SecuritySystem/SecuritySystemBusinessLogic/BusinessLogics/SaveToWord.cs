using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SecuritySystemBusinessLogic.HelperModels;
using System.Collections.Generic;

namespace SecuritySystemBusinessLogic.BusinessLogics
{
    public static class SaveToWord
    {
        // Создание документа
        public static void CreateDoc(WordInfo info)
        {
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());

                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordParagraphProperties)> { (info.Title,
                    new WordParagraphProperties
                    {
                        Bold = true,
                        Size = "24", }
                    ) },
                    TextProperties = new WordParagraphProperties
                    {
                        Size = "24",
                        JustificationValues = JustificationValues.Center
                    }
                }));

                foreach (var secure in info.Secures)
                {
                    docBody.AppendChild(CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordParagraphProperties)> {(
                        secure.SecureName + " ",
                        new WordParagraphProperties
                        {
                            Size = "24",
                            Bold =true
                        }),
                        (
                        secure.Price.ToString(),
                        new WordParagraphProperties
                        {
                            Size = "24",
                        }
                        )},

                        TextProperties = new WordParagraphProperties
                        {
                            Size = "24",
                            JustificationValues = JustificationValues.Both
                        }
                    }));
                }
                docBody.AppendChild(CreateSectionProperties());

                wordDocument.MainDocumentPart.Document.Save();
            }
        }

        // Настройка страницы
        private static SectionProperties CreateSectionProperties()
        {
            SectionProperties properties = new SectionProperties();

            PageSize pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };

            properties.AppendChild(pageSize);

            return properties;
        }

        // Создание абзаца с текстом 
        private static Paragraph CreateParagraph(WordParagraph paragraph)
        {
            if (paragraph != null)
            {
                Paragraph docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));

                foreach (var run in paragraph.Texts)
                {
                    Run docRun = new Run();

                    RunProperties properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = run.Item2.Size });
                    if (run.Item2.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }
                    docRun.AppendChild(properties);

                    docRun.AppendChild(new Text
                    {
                        Text = run.Item1,
                        Space = SpaceProcessingModeValues.Preserve
                    });

                    docParagraph.AppendChild(docRun);
                }
                return docParagraph;
            }
            return null;
        }

        // Задание форматирования для абзаца
        private static ParagraphProperties CreateParagraphProperties(WordParagraphProperties paragraphProperties)
        {
            if (paragraphProperties != null)
            {
                ParagraphProperties properties = new ParagraphProperties();

                properties.AppendChild(new Justification()
                {
                    Val = paragraphProperties.JustificationValues
                });

                properties.AppendChild(new SpacingBetweenLines
                {
                    LineRule = LineSpacingRuleValues.Auto
                });

                properties.Append(new Indentation());

                ParagraphMarkRunProperties paragraphMarkRunProperties =
                    new ParagraphMarkRunProperties();
                if (!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize
                    {
                        Val = paragraphProperties.Size
                    });
                }
                properties.AppendChild(paragraphMarkRunProperties);

                return properties;
            }
            return null;
        }
    }
}