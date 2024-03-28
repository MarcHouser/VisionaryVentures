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

        // Instantiate connection string
        private static readonly String? LabOneDBConnectionString =
            "Server=localhost;Database=Lab4;Trusted_Connection=True";

        private static readonly String? AuthConnString =
            "Server=Localhost;Database=AUTH;Trusted_Connection=True";

        // Method to read user information
        public static SqlDataReader UserReader()
        {
            SqlCommand CmdUserRead = new SqlCommand();
            CmdUserRead.Connection = LabOneDBConnection;
            CmdUserRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdUserRead.CommandText = "SELECT * FROM Users";
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

        // Method to read message information
        public static SqlDataReader MessageReader(int? CollaborationID)
        {
            SqlCommand CmdMessageRead = new SqlCommand();
            CmdMessageRead.Connection = LabOneDBConnection;
            CmdMessageRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdMessageRead.CommandText = @"SELECT Messages.*, Users.FirstName, Users.LastName FROM Messages INNER JOIN Users ON Messages.SentFrom = Users.UserID 
                                           JOIN Collaborations c ON Messages.CollaborationID = c.CollaborationID WHERE c.CollaborationID = @CollaborationID";
            CmdMessageRead.Parameters.AddWithValue("@CollaborationID", CollaborationID);
            CmdMessageRead.Connection.Open();

            SqlDataReader MessageReader = CmdMessageRead.ExecuteReader();

            return MessageReader;
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
            cmdSPLogin.CommandText = "sp_Lab4Login";
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
