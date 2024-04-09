using System.Data.SqlClient;
using VisionaryVentures.Pages.DataClasses;
using VisionaryVentures.Pages.DB;

namespace VisionaryVentures.Pages.DB
{
    public class DBClassReaders
    {

        // Create SQL connection
        public static SqlConnection LabOneDBConnection = new SqlConnection();
        public static SqlConnection AuthConn = new SqlConnection();

        private static readonly String? LabOneDBConnectionString =
            "Server=tcp:visionaryventures.database.windows.net,1433;" +
            "Initial Catalog=Sprint3;" +
            "Persist Security Info=False;" +
            "User ID=VisionaryVenturesAdmin;" +
            "Password=COB484Capstone;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=False;" +
            "Connection Timeout=30;";
        private static readonly String? AuthConnString =
            "Server=tcp:visionaryventures.database.windows.net,1433;" +
            "Initial Catalog=AUTH;" +
            "Persist Security Info=False;" +
            "User ID=VisionaryVenturesAdmin;" +
            "Password=COB484Capstone;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=False;" +
            "Connection Timeout=30;";

        // Method to read user information
        public static SqlDataReader UserReader()
        {
            SqlCommand CmdUserRead = new SqlCommand();
            CmdUserRead.Connection = LabOneDBConnection;
            CmdUserRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdUserRead.CommandText = "SELECT * FROM Users JOIN Accounts ON Users.AccountID = Accounts.AccountID WHERE Accounts.UserType = 2";
            CmdUserRead.Connection.Open();

            SqlDataReader UserReader = CmdUserRead.ExecuteReader();

            return UserReader;
        }

        // Method to read Dataset information
        public static SqlDataReader DatasetReader()
        {
            SqlCommand CmdDatasetRead = new SqlCommand();
            CmdDatasetRead.Connection = LabOneDBConnection;
            CmdDatasetRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdDatasetRead.CommandText = "SELECT * FROM Datasets";
            CmdDatasetRead.Connection.Open();

            SqlDataReader DatasetReader = CmdDatasetRead.ExecuteReader();

            return DatasetReader;
        }

        // Method to read Chat information
        public static SqlDataReader ChatReader()
        {
            SqlCommand CmdChatRead = new SqlCommand();
            CmdChatRead.Connection = LabOneDBConnection;
            CmdChatRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdChatRead.CommandText = "SELECT * FROM Chats";
            CmdChatRead.Connection.Open();

            SqlDataReader ChatReader = CmdChatRead.ExecuteReader();

            return ChatReader;
        }

        // Method to read message information
        public static SqlDataReader MessageReader(int? ChatID)
        {
            SqlCommand CmdMessageRead = new SqlCommand();
            CmdMessageRead.Connection = LabOneDBConnection;
            CmdMessageRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdMessageRead.CommandText = @"SELECT Messages.*, Users.FirstName, Users.LastName FROM Messages INNER JOIN Users ON Messages.SentFrom = Users.UserID 
                                           JOIN Chats c ON Messages.ChatID = c.ChatID WHERE c.ChatID = @ChatID";
            CmdMessageRead.Parameters.AddWithValue("@ChatID", ChatID);
            CmdMessageRead.Connection.Open();

            SqlDataReader MessageReader = CmdMessageRead.ExecuteReader();

            return MessageReader;
        }

        // Method to read knowledge groups by user
        public static SqlDataReader KnowledgeGroupReaderByUser(int? UserID)
        {
            SqlCommand CmdKnowledgeGroupRead = new SqlCommand();
            CmdKnowledgeGroupRead.Connection = LabOneDBConnection;
            CmdKnowledgeGroupRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdKnowledgeGroupRead.CommandText = @"SELECT kg.KnowledgeGroupID, kg.Title, kg.Description FROM KnowledgeGroups kg JOIN KnowledgeGroupParticipants kgp 
                                                ON kg.KnowledgeGroupID = kgp.KnowledgeGroupID WHERE kgp.UserID = @UserID";
            CmdKnowledgeGroupRead.Parameters.AddWithValue("@UserID", UserID);
            CmdKnowledgeGroupRead.Connection.Open();

            SqlDataReader KnowledgeGroupReader = CmdKnowledgeGroupRead.ExecuteReader();

            return KnowledgeGroupReader;
        }

        // Method to read knowledge groups
        public static SqlDataReader KnowledgeGroupReader()
        {
            SqlCommand CmdKnowledgeGroupRead = new SqlCommand();
            CmdKnowledgeGroupRead.Connection = LabOneDBConnection;
            CmdKnowledgeGroupRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdKnowledgeGroupRead.CommandText = "SELECT * FROM KnowledgeGroups";
            CmdKnowledgeGroupRead.Connection.Open();

            SqlDataReader KnowledgeGroupReader = CmdKnowledgeGroupRead.ExecuteReader();

            return KnowledgeGroupReader;
        }

        // Get reports by knowledge group
        public static SqlDataReader GetReportsByKnowledgeGroup(int KnowledgeGroupID)
        {
            SqlCommand cmdReportsByKnowledgeGroup = new SqlCommand();
            cmdReportsByKnowledgeGroup.Connection = LabOneDBConnection;
            cmdReportsByKnowledgeGroup.Connection.ConnectionString = LabOneDBConnectionString;
            cmdReportsByKnowledgeGroup.CommandText = @"SELECT * FROM Reports 
                                                     JOIN SwotAnalysis ON Reports.ReportID = SwotAnalysis.ReportID
                                                     JOIN PestAnalysis ON Reports.ReportID = PestAnalysis.ReportID       
                                                     WHERE Reports.KnowledgeGroupID = @KnowledgeGroupID";
            cmdReportsByKnowledgeGroup.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);
            cmdReportsByKnowledgeGroup.Connection.Open();

            SqlDataReader ReportsByKnowledgeGroup = cmdReportsByKnowledgeGroup.ExecuteReader();

            return ReportsByKnowledgeGroup;
        }

        // Get reports with analysis by knowledge group
        public static SqlDataReader GetReportsWithAnalysisByKnowledgeGroup(int KnowledgeGroupID)
        {
            SqlCommand cmdReportsByKnowledgeGroup = new SqlCommand();
            cmdReportsByKnowledgeGroup.Connection = LabOneDBConnection;
            cmdReportsByKnowledgeGroup.Connection.ConnectionString = LabOneDBConnectionString;
            cmdReportsByKnowledgeGroup.CommandText = @"SELECT * FROM ReportsWithDataAnalysis 
                                                     JOIN SwotWithDataAnalysis ON ReportsWithDataAnalysis.ReportID = SwotWithDataAnalysis.ReportID
                                                     JOIN PestWithDataAnalysis ON ReportsWithDataAnalysis.ReportID = PestWithDataAnalysis.ReportID
                                                     WHERE ReportsWithDataAnalysis.KnowledgeGroupID = @KnowledgeGroupID";
            cmdReportsByKnowledgeGroup.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);
            cmdReportsByKnowledgeGroup.Connection.Open();

            SqlDataReader ReportsByKnowledgeGroup = cmdReportsByKnowledgeGroup.ExecuteReader();

            return ReportsByKnowledgeGroup;
        }

        // Get reports by report ID
        public static SqlDataReader GetReportsByReportID(int ReportID)
        {
            SqlCommand cmdReportsByReportID = new SqlCommand();
            cmdReportsByReportID.Connection = LabOneDBConnection;
            cmdReportsByReportID.Connection.ConnectionString = LabOneDBConnectionString;
            cmdReportsByReportID.CommandText = @"SELECT * FROM Reports 
                                                JOIN SwotAnalysis ON Reports.ReportID = SwotAnalysis.ReportID
                                                JOIN PestAnalysis ON Reports.ReportID = PestAnalysis.ReportID
                                                WHERE Reports.ReportID = @ReportID";
            cmdReportsByReportID.Parameters.AddWithValue("@ReportID", ReportID);
            cmdReportsByReportID.Connection.Open();

            SqlDataReader ReportsByReportID = cmdReportsByReportID.ExecuteReader();

            return ReportsByReportID;
        }

        // Get reports with analysis by report id
        public static SqlDataReader GetReportsWithAnalysisByReportID(int ReportID)
        {
            SqlCommand cmdReportsByReportID = new SqlCommand();
            cmdReportsByReportID.Connection = LabOneDBConnection;
            cmdReportsByReportID.Connection.ConnectionString = LabOneDBConnectionString;
            cmdReportsByReportID.CommandText = @"SELECT * FROM ReportsWithDataAnalysis
                                                JOIN SwotWithDataAnalysis ON ReportsWithDataAnalysis.ReportID = SwotWithDataAnalysis.ReportID
                                                JOIN PestWithDataAnalysis ON ReportsWithDataAnalysis.ReportID = PestWithDataAnalysis.ReportID
                                                WHERE ReportsWithDataAnalysis.ReportID = @ReportID";
            cmdReportsByReportID.Parameters.AddWithValue("@ReportID", ReportID);
            cmdReportsByReportID.Connection.Open();

            SqlDataReader ReportsByReportID = cmdReportsByReportID.ExecuteReader();

            return ReportsByReportID;
        }

        public static SqlDataReader KnowledgeReader()
        {
            SqlCommand CmdKnowledgeRead = new SqlCommand();
            CmdKnowledgeRead.Connection = LabOneDBConnection;
            CmdKnowledgeRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdKnowledgeRead.CommandText = "SELECT * FROM KnowledgeItems";
            CmdKnowledgeRead.Connection.Open();

            SqlDataReader KnowledgeReader = CmdKnowledgeRead.ExecuteReader();

            return KnowledgeReader;
        }

        public static SqlDataReader SWOTAnalysisReader()
        {
            SqlCommand CmdSWOTRead = new SqlCommand();
            CmdSWOTRead.Connection = LabOneDBConnection;
            CmdSWOTRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdSWOTRead.CommandText = "SELECT * FROM SwotAnalyses";
            CmdSWOTRead.Connection.Open();

            SqlDataReader SWOTReader = CmdSWOTRead.ExecuteReader();

            return SWOTReader;
        }

        public static int LoginQuery(string loginQuery)
        {
            // This method expects to receive an SQL SELECT
            // query that uses the COUNT command.

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = LabOneDBConnection;
            cmdLogin.Connection.ConnectionString = LabOneDBConnectionString;
            cmdLogin.CommandText = loginQuery;
            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int rowCount = (int)cmdLogin.ExecuteScalar();

            return rowCount;
        }

        public static bool HashedParameterLogin(string Username, string Password)
        {
            string loginQuery =
                "SELECT PasswordHash FROM HashedCredentials WHERE Username = @Username";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = AuthConn;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);

            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            SqlDataReader hashReader = cmdLogin.ExecuteReader();
            if (hashReader.Read())
            {
                string correctHash = hashReader["PasswordHash"].ToString();

                if (PasswordHash.ValidatePassword(Password, correctHash))
                {
                    return true;
                }
            }

            return false;
        }

        // Method to get the UserID
        public static int GetUserID(string Username)
        {
            string loginQuery =
                "SELECT UserID FROM HashedCredentials WHERE Username = @Username";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = LabOneDBConnection;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);

            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int UserID = (int)cmdLogin.ExecuteScalar();

            cmdLogin.Connection.Close();

            return UserID;
        }

        // Method to get User Type
        public static int GetUserType(int UserID)
        {
            string userTypeQuery = "SELECT UserType FROM Accounts a JOIN Users u ON a.AccountID = u.AccountID WHERE u.UserID = @UserID";

            SqlCommand cmdUserType = new SqlCommand();
            cmdUserType.Connection = LabOneDBConnection;
            cmdUserType.Connection.ConnectionString = LabOneDBConnectionString;

            cmdUserType.CommandText = userTypeQuery;
            cmdUserType.Parameters.AddWithValue("@UserID", UserID);

            cmdUserType.Connection.Open();

            short UserType = (short)cmdUserType.ExecuteScalar();

            return UserType;
        }

        // Stored Procedure Login
        public static bool SPLogin(string Username)
        {
            SqlCommand cmdSPLogin = new SqlCommand();
            cmdSPLogin.Connection = new SqlConnection();
            cmdSPLogin.Connection.ConnectionString = AuthConnString;
            cmdSPLogin.CommandType = System.Data.CommandType.StoredProcedure;
            cmdSPLogin.Parameters.AddWithValue("@Username", Username);
            cmdSPLogin.CommandText = "sp_Sprint3Login";
            cmdSPLogin.Connection.Open();
            if (((int)cmdSPLogin.ExecuteScalar()) > 0)
            {
                return true;
            }

            return false;
        }

        public static string GetFirstNameByUsername(string username)
        {
            string firstName = "";
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string sqlQuery = "SELECT FirstName FROM User WHERE Username = @Username";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        firstName = reader["FirstName"].ToString();
                    }
                }
            }
            return firstName;
        }

        public static SqlDataReader GetDatasetByUser(int? UserID)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string sqlQuery = "SELECT * FROM Datasets WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.AddWithValue("@UserID", UserID);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                return reader;
            }
        }

        public static SqlDataReader GeneralReaderQuery(string sqlQuery)
        {
            SqlCommand cmdProductRead = new SqlCommand();
            cmdProductRead.Connection = LabOneDBConnection;
            cmdProductRead.Connection.ConnectionString =
            LabOneDBConnectionString;
            cmdProductRead.CommandText = sqlQuery;
            cmdProductRead.Connection.Open();
            SqlDataReader tempReader = cmdProductRead.ExecuteReader();
            return tempReader;
        }
    }
}
