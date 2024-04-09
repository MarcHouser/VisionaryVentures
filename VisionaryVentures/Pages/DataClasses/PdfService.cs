using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;

namespace VisionaryVentures.Pages.DataClasses
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)

        {

            _converter = converter;

        }

        public byte[] GeneratePdf(string htmlContent)

        {

            var globalSettings = new GlobalSettings

            {

                ColorMode = ColorMode.Color,

                Orientation = Orientation.Portrait,

                PaperSize = PaperKind.A4,

                Margins = new MarginSettings { Top = 10, Bottom = 10 }

            };

            var objectSettings = new ObjectSettings

            {

                PagesCount = true,

                HtmlContent = htmlContent,

                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "styles.css") },

                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },

                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Left = "Generated on " + DateTime.Now }

            };

            var document = new HtmlToPdfDocument()

            {

                GlobalSettings = globalSettings,

                Objects = { objectSettings }

            };

            return _converter.Convert(document);

        }
    }
}
