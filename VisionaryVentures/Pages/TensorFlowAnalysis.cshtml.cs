using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace VisionaryVentures.Pages
{
    public class TensorFlowAnalysisModel : PageModel
    {
        public string FileName { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> Records { get; set; }

        public void OnGet(string fileName)
        {
            FileName = fileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                LoadCsvData(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", fileName));
            }
        }

        private void LoadCsvData(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            Headers = csv.HeaderRecord.ToList();

            Records = new List<List<string>>();
            while (csv.Read())
            {
                var record = new List<string>();
                foreach (var header in Headers)
                {
                    record.Add(csv.GetField(header));
                }
                Records.Add(record);
            }
        }

        // You would include your TensorFlow analysis functions here.
        // These could be methods that load a TensorFlow model, prepare the dataset for the model,
        // perform predictions or other analyses, and format the results for display.
    }
}
