using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML.Data;
using MathNet.Numerics.Statistics;

namespace VisionaryVentures.Pages
{
    public class AnalyzeModel : PageModel
    {
        private readonly MLContext _mlContext;
        public AnalyzeModel()
        {
            _mlContext = new MLContext();
        }

        [BindProperty]
        public string FileName { get; set; }

        [BindProperty]
        public string AnalysisType { get; set; } // New property to store the type of analysis

        // Other properties as before

        public void OnGet()
        {
        }

        public IActionResult OnPostPerformAnalysis(string fileName, string analysisType)
        {
            FileName = fileName;
            AnalysisType = analysisType;

            // Path to the dataset
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", fileName);

            // Load the data
            IDataView dataView = _mlContext.Data.LoadFromTextFile(filePath, hasHeader: true, separatorChar: ',');

            // Switch between different types of analyses based on user input
            switch (analysisType.ToLower())
            {
                case "statistics":
                    CalculateBasicStatistics(dataView);
                    break;
                case "regression":
                    // Placeholder for regression analysis
                    // PerformRegressionAnalysis(dataView);
                    break;
                // Add more cases for other types of analysis
                default:
                    ViewData["ErrorMessage"] = "Analysis type not supported.";
                    return Page();
            }

            return Page();
        }

        private void CalculateBasicStatistics(IDataView dataView)
        {
            // Implementation remains the same as before
        }

        // Placeholder for regression analysis method
        // private void PerformRegressionAnalysis(IDataView dataView)
        // {
        //     // Implement regression analysis here
        // }
    }
}
