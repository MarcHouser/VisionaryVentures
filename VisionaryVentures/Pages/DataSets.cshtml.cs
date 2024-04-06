using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisionaryVentures.Pages.DataClasses;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;
using VisionaryVentures.Pages.DB;
using System.Text.RegularExpressions;
using ExcelDataReader;
using System.Data;

namespace VisionaryVentures.Pages
{
    public class DataSetsModel : PageModel
    {

        [BindProperty]
        public IFormFile UploadedFile { get; set; }
       

        public List<dynamic> Data { get; set; } = new List<dynamic>();
        [BindProperty]
        public List<IFormFile> files { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> Records { get; set; }

        public List<string> DataSetFiles { get; set; } = new List<string>();

        public string SelectedFileName { get; set; }

        private readonly string _connectionString = 
            "Server=tcp:visionaryventures.database.windows.net,1433;" +
            "Initial Catalog=Sprint3;" +
            "Persist Security Info=False;" +
            "User ID=VisionaryVenturesAdmin;" +
            "Password=COB484Capstone;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=False;" +
            "Connection Timeout=30;";



        // GET: /DataSets
        public async Task OnGetAsync()
        {

            string datasetDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset");

            DirectoryInfo datasetFolder = new DirectoryInfo(datasetDir);
            var fileListing = datasetFolder.GetFiles();

            foreach (var file in fileListing)
            {
                DataSetFiles.Add(file.Name);
            }
        }

        //public async Task OnGetReadCsvAsync(string fileName)
        //{
        //    SelectedFileName = fileName;

        //    PopulateDataSetFiles();

        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", fileName);
        //    using var reader = new StreamReader(filePath);
        //    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        //    csv.Read();
        //    csv.ReadHeader();
        //    Headers = csv.HeaderRecord.ToList();

        //    Records = new List<List<string>>();
        //    while (csv.Read())
        //    {
        //        var record = new List<string>();
        //        foreach (var header in Headers)
        //        {
        //            record.Add(csv.GetField(header));
        //        }
        //        Records.Add(record);
        //    }
        //}


        public async Task OnGetReadFileAsync(string fileName)
        {
            HttpContext.Session.SetString("fileName", fileName);
            SelectedFileName = HttpContext.Session.GetString(fileName);

            PopulateDataSetFiles();

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

        //public async Task<IActionResult> OnGetDeleteAsync(string fileName)
        //{

        //    var sanitizedFileName = Path.GetFileNameWithoutExtension(fileName); // Remove the extension
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", fileName);

        //    if (System.IO.File.Exists(filePath))
        //    {
        //        // Delete the file from the file system
        //        System.IO.File.Delete(filePath);

        //        // Delete the corresponding table from the database
        //        string tableName = $"Dataset_{sanitizedFileName}";
        //        string dropTableSql = $"DROP TABLE IF EXISTS [{tableName}];";
        //        await ExecuteSqlNonQuery(dropTableSql);

        //        // Optionally, remove any references from a dataset registry if you have one
        //        // This step depends on how you are tracking datasets in your application.

        //        return RedirectToPage(new { successMessage = "Dataset deleted successfully." });
        //    }
        //    else
        //    {
        //        return RedirectToPage(new { errorMessage = "File not found." });
        //    }
        //}

        public IActionResult OnPostDeleteFile(string fileName)
        {
            // Delete table from SQL database
            string tableName = Path.GetFileNameWithoutExtension(fileName).Replace(" ", "_");
            string deleteTableQuery = $"DROP TABLE IF EXISTS [Dataset_{tableName}]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Delete file from "Uploads" folder
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset");
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Redirect back to the page
            return RedirectToPage();
        }



        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var filePaths = new List<string>();
        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", formFile.FileName);
        //            filePaths.Add(filePath);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                formFile.CopyTo(stream);
        //            }

        //            //await CreateTableFromCSV(filePath, formFile.FileName);

        //            DBClassWriters.AddDataset((int)HttpContext.Session.GetInt32("userid"), formFile.FileName, DateTime.Now);

        //            await ProcessCSVAndCreateTable(filePath, formFile.FileName);
        //        }
        //    }
        //    return RedirectToPage();
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", formFile.FileName);
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    // Check file extension and call the appropriate processing method
                    var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
                    switch (fileExtension)
                    {
                        case ".csv":
                            await ProcessCSVAndCreateTable(filePath, formFile.FileName);
                            break;
                        case ".xls":
                        case ".xlsx":
                            await ProcessExcelAndCreateTable(filePath, formFile.FileName);
                            break;
                        default:
                            throw new InvalidOperationException("Unsupported file format.");
                    }

                    DBClassWriters.AddDataset((int)HttpContext.Session.GetInt32("userid"), formFile.FileName, DateTime.Now, "No Description");
                }
            }
            return RedirectToPage();
        }

        private async Task ProcessExcelAndCreateTable(string filePath, string fileName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataReader.ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Use the first row as headers
                        }
                    });

                    DataTable dataTable = result.Tables[0];
                    Dictionary<string, string> columnDataTypes = InferColumnDataTypesFromExcel(dataTable);

                    string tableName = SanitizeFileNameForTableName(fileName);
                    string createTableSql = BuildCreateTableSql(tableName, dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray(), columnDataTypes);
                    await ExecuteSqlNonQuery(createTableSql);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string insertSql = BuildInsertSql(tableName, dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray(), row.ItemArray, columnDataTypes);
                        await ExecuteSqlNonQuery(insertSql);
                    }
                }
            }
        }

        private Dictionary<string, string> InferColumnDataTypesFromExcel(DataTable dataTable)
        {
            var columnTypes = new Dictionary<string, string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                // Default to string data type
                columnTypes[column.ColumnName] = "NVARCHAR(MAX)";
            }
            // Optionally, implement more sophisticated type inference based on actual data
            return columnTypes;
        }

        private string BuildInsertSql(string tableName, string[] headers, object[] values, Dictionary<string, string> columnTypes)
        {
            var valueList = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                var valueType = columnTypes[headers[i]];
                string valueStr = valueType == "NVARCHAR(MAX)" ? $"'{values[i].ToString().Replace("'", "''")}'" : values[i].ToString();
                valueList.Add(valueStr);
            }

            var columns = string.Join(", ", headers.Select(header => $"[{header}]"));
            var valuesString = string.Join(", ", valueList);
            return $"INSERT INTO [{tableName}] ({columns}) VALUES ({valuesString});";
        }

        private async Task InsertDataIntoTable(string tableName, List<List<object>> data, string[] headers)
        {
            foreach (var row in data)
            {
                var columns = string.Join(", ", headers.Select(header => $"[{header}]"));
                var values = string.Join(", ", row.Select(value => $"'{value.ToString().Replace("'", "''")}'"));
                string insertSql = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values});";
                await ExecuteSqlNonQuery(insertSql);
            }
        }


        // Method to read Excel file without treating the first row as headers
        //private List<List<object>> ReadExcel(string filePath)
        //{
        //    using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //    using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
        //    {
        //        var result = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration()
        //        {
        //            ConfigureDataTable = (_) => new ExcelDataReader.ExcelDataTableConfiguration()
        //            {
        //                UseHeaderRow = false // Do not use the first row as header
        //            }
        //        });

        //        DataTable dataTable = result.Tables[0];
        //        List<List<object>> data = new List<List<object>>();

        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            List<object> rowData = row.ItemArray.ToList();
        //            data.Add(rowData);
        //        }

        //        return data;
        //    }
        //}

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


        public IActionResult OnPostAnalyzeDataset(string fileName)
        {
            return RedirectToPage("./TensorFlowAnalysis", fileName);
        }

        private async Task ProcessCSVAndCreateTable(string filePath, string fileName)
        {
            // Infer data types here
            var columnDataTypes = InferColumnDataTypes(filePath);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;

            string tableName = SanitizeFileNameForTableName(fileName);

            // Pass columnDataTypes to BuildCreateTableSql
            string createTableSql = BuildCreateTableSql(tableName, headers, columnDataTypes);
            await ExecuteSqlNonQuery(createTableSql);

            while (csv.Read())
            {
                string insertSql = BuildInsertSql(tableName, headers, csv);
                await ExecuteSqlNonQuery(insertSql);
            }
        }

        private string SanitizeFileNameForTableName(string fileName)
        {
            string sanitized = Path.GetFileNameWithoutExtension(fileName);
            sanitized = Regex.Replace(sanitized, "[^a-zA-Z0-9_]", "");
            return $"Dataset_{sanitized}";
        }

        private string BuildCreateTableSql(string tableName, string[] headers, Dictionary<string, string> columnTypes)
        {
            var columns = headers.Select(header => $"[{header}] {columnTypes[header]}").ToArray();
            return $"CREATE TABLE [{tableName}] ({string.Join(", ", columns)});";
        }

        private string BuildInsertSql(string tableName, string[] headers, IReaderRow csv)
        {
            var columns = string.Join(", ", headers.Select(header => $"[{header}]"));
            var values = string.Join(", ", headers.Select(header => $"'{csv.GetField(header).Replace("'", "''")}'"));
            return $"INSERT INTO [{tableName}] ({columns}) VALUES ({values});";
        }

        private async Task ExecuteSqlNonQuery(string sqlCommandText)
        {
            using (var connection = new SqlConnection("Server=localhost;Database=Sprint2;Trusted_Connection=True"))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sqlCommandText, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            connection.Close();
            }
        }

        private void PopulateDataSetFiles()
        {
            string datasetDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset");
            DirectoryInfo datasetFolder = new DirectoryInfo(datasetDir);
            var fileListing = datasetFolder.GetFiles();

            DataSetFiles.Clear(); // Ensure the list is clear before adding to it to prevent duplicates

            foreach (var file in fileListing)
            {
                DataSetFiles.Add(file.Name);
            }
        }

        private Dictionary<string, string> InferColumnDataTypes(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;
            var columnTypes = new Dictionary<string, string>(headers.Length);

            foreach (var header in headers)
            {
                columnTypes[header] = "INT"; // Assume INT initially for all
            }

            int rowCount = 0;
            while (csv.Read() && rowCount < 100) // Scan up to 100 rows for type inference
            {
                foreach (var header in headers)
                {
                    var field = csv.GetField(header);
                    if (columnTypes[header] == "INT" && !int.TryParse(field, out _))
                    {
                        // If not INT, downgrade to FLOAT
                        columnTypes[header] = "FLOAT";
                    }
                    if (columnTypes[header] == "FLOAT" && !float.TryParse(field, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    {
                        // If not FLOAT, downgrade to NVARCHAR
                        columnTypes[header] = "NVARCHAR(MAX)";
                    }
                }
                rowCount++;
            }

            return columnTypes;
        }

    }
}
