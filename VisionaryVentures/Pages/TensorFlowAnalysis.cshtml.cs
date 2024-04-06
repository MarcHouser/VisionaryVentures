using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using ExcelDataReader;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VisionaryVentures.Pages.DB;
using VisionaryVentures.Pages.DataClasses;

namespace VisionaryVentures.Pages
{
    public class TensorFlowAnalysisModel : PageModel
    {
        public string FileName { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> Records { get; set; }
        public string SelectedFileName { get; set; }

        [BindProperty]
        public string DependentVariable { get; set; }
        [BindProperty]
        public string IndependentVariables { get; set; }

        public string Output { get; set; }

        public List<KnowledgeGroup> AllGroups { get; set; } = new List<KnowledgeGroup>();

        [BindProperty]
        public int SelectedKGID { get; set; }

        // Report Properties
        [BindProperty]
        public string ReportTitle { get; set; }
        [BindProperty]
        public string ReportDescription { get; set; }

        //SWOT Properties
        [BindProperty]
        public string SWOTStrengths { get; set; }
        [BindProperty]
        public string SWOTWeaknesses { get; set; }
        [BindProperty]
        public string SWOTOpportunities { get; set; }
        [BindProperty]
        public string SWOTThreats { get; set; }
        [BindProperty]
        public string SWOTImplications { get; set; }
        [BindProperty]
        public string Strategy { get; set; }
        [BindProperty]
        public string SWOTNotes { get; set; }

        //PEST Properties
        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public string Factor { get; set; }
        [BindProperty]
        public string PESTImplications { get; set; }
        [BindProperty]
        public string PossibleActions { get; set; }
        [BindProperty]
        public string PESTNotes { get; set; }

        public string NewPlotName { get; set; }

        public async Task OnGetAsync()
        {
            using (var reader = DBClassReaders.KnowledgeGroupReaderByUser(HttpContext.Session.GetInt32("userid")))
            {
                while (reader.Read())
                {
                    AllGroups.Add(new KnowledgeGroup
                    {
                        KnowledgeGroupID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }

                DBClassReaders.LabOneDBConnection.Close();
            }
        }
        
        public async Task OnGetReadFileAsync(string fileName)
        {
            SelectedFileName = HttpContext.Session.GetString(fileName);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", fileName);

            // Check the file extension and call the appropriate method
            if (Path.GetExtension(fileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
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
            else if (Path.GetExtension(fileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                     Path.GetExtension(fileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
            {
                // Use ReadExcelAndInferDataTypes method to read the data and infer column types
                var (excelData, columnTypes) = ReadExcelAndInferDataTypes(filePath);

                // Assuming the first row contains headers
                Headers = columnTypes.Keys.ToList();
                Records = new List<List<string>>();

                // Convert all data to string type as Records is a list of list of strings
                foreach (var row in excelData)
                {
                    var record = row.Select(cellValue => cellValue?.ToString() ?? string.Empty).ToList();
                    Records.Add(record);
                }
            }
            else
            {
                // Handle unsupported file types or add logic to deal with other file formats
                throw new InvalidOperationException("The file format is not supported.");
            }
        }

        private (List<List<object>> data, Dictionary<string, string> columnTypes) ReadExcelAndInferDataTypes(string filePath)
        {
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataReader.ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true // Use the first row as header
                    }
                });

                DataTable dataTable = result.Tables[0];
                Dictionary<string, string> columnTypes = InitializeColumnDataTypes(dataTable.Columns);
                List<List<object>> data = new List<List<object>>();

                foreach (DataRow row in dataTable.Rows)
                {
                    List<object> rowData = new List<object>();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        object cellValue = row[i];
                        rowData.Add(cellValue);

                        // Infer data type
                        string columnName = dataTable.Columns[i].ColumnName;
                        InferColumnType(columnTypes, columnName, cellValue);
                    }
                    data.Add(rowData);
                }

                return (data, columnTypes);
            }
        }

        private Dictionary<string, string> InitializeColumnDataTypes(DataColumnCollection columns)
        {
            var columnTypes = new Dictionary<string, string>();
            foreach (DataColumn column in columns)
            {
                // Initialize all columns as INT
                columnTypes[column.ColumnName] = "INT";
            }
            return columnTypes;
        }

        private void InferColumnType(Dictionary<string, string> columnTypes, string columnName, object cellValue)
        {
            // If the current type is already NVARCHAR(MAX), no need to check further
            if (columnTypes[columnName] == "NVARCHAR(MAX)") return;

            // If it's not an INT, check if it's a FLOAT or NVARCHAR
            if (cellValue != null && !int.TryParse(cellValue.ToString(), out _))
            {
                if (float.TryParse(cellValue.ToString(), out _))
                {
                    // Upgrade to FLOAT if it's currently INT
                    if (columnTypes[columnName] == "INT") columnTypes[columnName] = "FLOAT";
                }
                else
                {
                    // Upgrade to NVARCHAR(MAX) if it's not a valid float
                    columnTypes[columnName] = "NVARCHAR(MAX)";
                }
            }
        }

        public void OnPostStartRegression()
        {
            SelectedFileName = HttpContext.Session.GetString("fileName");

            // Assuming filePath is stored in session or determined here
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", SelectedFileName);
            string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "analysis", "RegressionAnalysis.py");

            // Ensure Python executable path is correct. This might vary based on your server setup.
            //string pythonExecutable = @"C:\home\python3111x64\python.exe";
            string pythonExecutable = "python.exe";

            string plotName = $"analysis_{DateTime.Now.Ticks}.png";
            NewPlotName = plotName;
            string analysisText = $"Analysis for {SelectedFileName}_{DateTime.Now.Ticks}";
            string arguments = $"\"{scriptPath}\" \"{filePath}\" \"{DependentVariable}\" \"{IndependentVariables}\" \"{plotName}\" \"{analysisText}\"";
            Output = RunPythonScript(pythonExecutable, arguments);
        }

        private string RunPythonScript(string pythonExecutable, string arguments)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonExecutable,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string stderr = process.StandardError.ReadToEnd(); // Capturing errors
                    string result = reader.ReadToEnd();
                    return result + "\nErrors: " + stderr;
                }
            }
        }

        public IActionResult OnGetDownloadOutput()
        {
            // Define the path to the file you want to download
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "analysis_output", "AnalysisOutput.txt");
            string fileName = "AnalysisOutput.txt"; // This is the name the downloaded file will have

            // Read the file's contents
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Return the file
            return File(fileBytes, "text/plain", fileName);
        }

        public async Task<IActionResult> OnPostSaveReportAsync()
        {
            string outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "analysis_output", $"Analysis for {SelectedFileName} - {DateTime.Now}");
            string imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "analysis_output", $"analysis_{DateTime.Now.Ticks}.png");

            DBClassWriters.BuildReportWithDataAnalysis(DateTime.Now, ReportTitle, ReportDescription, SWOTImplications, Strategy, DateTime.Now, SWOTNotes, SelectedKGID, SWOTStrengths,
                SWOTWeaknesses, SWOTOpportunities, SWOTThreats, Category, Factor, PESTImplications, PossibleActions, DateTime.Now, PESTNotes, outputFilePath, imageFilePath);
            
            DBClassWriters.LabOneDBConnection.Close();

            return RedirectToPage("/Reports");
        }
    }
}
