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
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;
using System.Text.RegularExpressions;

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
        public List<CityInfo> Cities { get; set; } = new List<CityInfo>();

        public List<string> DataSetFiles { get; set; } = new List<string>();

        public string SelectedFileName { get; set; }



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

        public async Task OnGetReadCsvAsync(string fileName)
        {
            SelectedFileName = fileName;

            PopulateDataSetFiles();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", fileName);
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

        public async Task <IActionResult> OnPostAsync()
        {
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if(formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "dataset", formFile.FileName);
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }

                    //await CreateTableFromCSV(filePath, formFile.FileName);

                    DBClassWriters.AddDataset((int)HttpContext.Session.GetInt32("userid"), formFile.FileName, DateTime.Now);

                    await ProcessCSVAndCreateTable(filePath, formFile.FileName);
                }
            }
            return RedirectToPage();
        }

        private async Task ProcessCSVAndCreateTable(string filePath, string fileName)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;

            // Generate a sanitized table name based on the file name
            string tableName = SanitizeFileNameForTableName(fileName);

            // Create the table
            string createTableSql = BuildCreateTableSql(tableName, headers);
            await ExecuteSqlNonQuery(createTableSql);

            // Insert data into the table
            while (csv.Read())
            {
                string insertSql = BuildInsertSql(tableName, headers, csv);
                await ExecuteSqlNonQuery(insertSql);
            }
        }

        private string SanitizeFileNameForTableName(string fileName)
        {
            // Simple example; adapt as needed for robustness
            string sanitized = Path.GetFileNameWithoutExtension(fileName);
            sanitized = Regex.Replace(sanitized, "[^a-zA-Z0-9_]", "");
            return $"Dataset_{sanitized}";
        }

        private string BuildCreateTableSql(string tableName, string[] headers)
        {
            var columns = headers.Select(header => $"[{header}] NVARCHAR(MAX)").ToArray();
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
            using (var connection = new SqlConnection("Server=localhost;Database=Lab4;Trusted_Connection=True"))
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
    }
}
