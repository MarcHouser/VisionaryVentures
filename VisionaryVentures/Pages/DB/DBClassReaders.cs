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

        // Methid to read dataset by collaboration
        public static SqlDataReader DatasetByCollaboration(int? CollaborationID)
        {
            SqlCommand CmdDatasetByCollabReader = new SqlCommand();
            CmdDatasetByCollabReader.Connection = LabOneDBConnection;
            CmdDatasetByCollabReader.Connection.ConnectionString = LabOneDBConnectionString;
            CmdDatasetByCollabReader.CommandText = @"SELECT d.* FROM Datasets d 
                                                    INNER JOIN CollaborationData cd ON d.DatasetID = cd.DatasetID 
                                                    JOIN Collaborations c ON cd.CollaborationID = c.CollaborationID
                                                    WHERE c.CollaborationID = @CollaborationID";
            CmdDatasetByCollabReader.Parameters.AddWithValue("@CollaborationID", CollaborationID);
            CmdDatasetByCollabReader.Connection.Open();

            SqlDataReader DatasetByCollabReader = CmdDatasetByCollabReader.ExecuteReader();

            return DatasetByCollabReader;
        }

        // Method to read analysis information
        public static SqlDataReader AnalysisReader()
        {
            SqlCommand CmdAnalysisRead = new SqlCommand();
            CmdAnalysisRead.Connection = LabOneDBConnection;
            CmdAnalysisRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdAnalysisRead.CommandText = "SELECT * FROM Analyses";
            CmdAnalysisRead.Connection.Open();

            SqlDataReader AnalysisReader = CmdAnalysisRead.ExecuteReader();

            return AnalysisReader;
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

        public static SqlDataReader PlanByCollaboration(int? CollaborationID)
        {
            SqlCommand CmdPlanByCollabReader = new SqlCommand();
            CmdPlanByCollabReader.Connection = LabOneDBConnection;
            CmdPlanByCollabReader.Connection.ConnectionString = LabOneDBConnectionString;
            CmdPlanByCollabReader.CommandText = @"SELECT pc.PlanContentsID, pc.PlanID, pc.PlanStep, pc.ContentDescription FROM PlanContents pc 
                                                  INNER JOIN Plans p ON pc.PlanID = p.PlanID 
                                                  INNER JOIN CollaborationPlans cp ON p.PlanID = cp.PlanID 
                                                  JOIN Collaborations c ON cp.CollaborationID = c.CollaborationID
                                                  WHERE c.CollaborationID = @CollaborationID";
            CmdPlanByCollabReader.Parameters.AddWithValue("@CollaborationID", CollaborationID);
            CmdPlanByCollabReader.Connection.Open();

            SqlDataReader PlanByCollabReader = CmdPlanByCollabReader.ExecuteReader();

            return PlanByCollabReader;
        }

        public static SqlDataReader KnowledgeByCollaboration(int? CollaborationID)
        {
            SqlCommand CmdKnowledgeByCollabReader = new SqlCommand();
            CmdKnowledgeByCollabReader.Connection = LabOneDBConnection;
            CmdKnowledgeByCollabReader.Connection.ConnectionString = LabOneDBConnectionString;
            CmdKnowledgeByCollabReader.CommandText = @"SELECT ki.* FROM KnowledgeItems ki 
                                                      INNER JOIN CollaborationKnowledge ck ON ki.KnowledgeItemID = ck.KnowledgeItemID 
                                                      JOIN Collaborations c ON ck.CollaborationID = c.CollaborationID
                                                      WHERE c.CollaborationID = @CollaborationID";
            CmdKnowledgeByCollabReader.Parameters.AddWithValue("@CollaborationID", CollaborationID);
            CmdKnowledgeByCollabReader.Connection.Open();

            SqlDataReader KnowledgeByCollabReader = CmdKnowledgeByCollabReader.ExecuteReader();

            return KnowledgeByCollabReader;
        }

        // Method to read knowledge information
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

        // Method to read plan infomration
        public static SqlDataReader PlanReader()
        {
            SqlCommand CmdPlanRead = new SqlCommand();
            CmdPlanRead.Connection = LabOneDBConnection;
            CmdPlanRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdPlanRead.CommandText = "SELECT * FROM Plans";
            CmdPlanRead.Connection.Open();

            SqlDataReader PlanReader = CmdPlanRead.ExecuteReader();

            return PlanReader;
        }

        // Method to read plan contents
        public static SqlDataReader PlanContentReader()
        {
            SqlCommand CmdPlanContentRead = new SqlCommand();
            CmdPlanContentRead.Connection = LabOneDBConnection;
            CmdPlanContentRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdPlanContentRead.CommandText = "SELECT * FROM PlanContents";
            CmdPlanContentRead.Connection.Open();

            SqlDataReader PlanContentReader = CmdPlanContentRead.ExecuteReader();

            return PlanContentReader;
        }

        // Method to read collaboration information
        public static SqlDataReader CollaborationReader()
        {
            SqlCommand CmdCollaborationRead = new SqlCommand();
            CmdCollaborationRead.Connection = LabOneDBConnection;
            CmdCollaborationRead.Connection.ConnectionString = LabOneDBConnectionString;
            CmdCollaborationRead.CommandText = "SELECT * FROM Collaborations";
            CmdCollaborationRead.Connection.Open();

            SqlDataReader CollaborationReader = CmdCollaborationRead.ExecuteReader();

            return CollaborationReader;
        }

        // Method to Read Collaboration by ID
        public static SqlDataReader CollaborationByIDReader(int? CollaborationID)
        {
            SqlCommand CmdCollaborationByID = new SqlCommand();
            CmdCollaborationByID.Connection = LabOneDBConnection;
            CmdCollaborationByID.Connection.ConnectionString = LabOneDBConnectionString;
            CmdCollaborationByID.CommandText = @"SELECT * FROM Collaborations WHERE CollaborationID = @CollaborationID";
            CmdCollaborationByID.Parameters.AddWithValue("@CollaborationID", CollaborationID);
            CmdCollaborationByID.Connection.Open();

            SqlDataReader CollaborationByID = CmdCollaborationByID.ExecuteReader();

            return CollaborationByID;
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

            return UserID;
        }

        // Stored Procedure Login
        public static bool SPLogin(string Username)
        {
            SqlCommand cmdSPLogin = new SqlCommand();
            cmdSPLogin.Connection = new SqlConnection();
            cmdSPLogin.Connection.ConnectionString = AuthConnString;
            cmdSPLogin.CommandType = System.Data.CommandType.StoredProcedure;
            cmdSPLogin.Parameters.AddWithValue("@Username", Username);
            cmdSPLogin.CommandText = "sp_Lab3Login";
            cmdSPLogin.Connection.Open();
            if (((int)cmdSPLogin.ExecuteScalar()) > 0)
            {
                return true;
            }

            return false;
        }

        // Check if a Plan is already associated with a Collaboration
        public static bool PlanIsAlreadyAssociated(int collaborationId, int planId)
        {
            using (var connection = new SqlConnection(LabOneDBConnectionString))
            {
                var query = "SELECT COUNT(1) FROM CollaborationPlans WHERE CollaborationID = @collaborationId AND PlanID = @planId";
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@collaborationId", collaborationId);
                    command.Parameters.AddWithValue("@planId", planId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Check if a Knowledge Item is already associated with a Collaboration
        public static bool KnowledgeItemIsAlreadyAssociated(int collaborationId, int knowledgeItemId)
        {
            using (var connection = new SqlConnection(LabOneDBConnectionString))
            {
                var query = "SELECT COUNT(1) FROM CollaborationKnowledge WHERE CollaborationID = @collaborationId AND KnowledgeItemID = @knowledgeItemId";
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@collaborationId", collaborationId);
                    command.Parameters.AddWithValue("@knowledgeItemId", knowledgeItemId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Check if a Dataset is already associated with a Collaboration
        public static bool DatasetIsAlreadyAssociated(int collaborationId, int datasetId)
        {
            using (var connection = new SqlConnection(LabOneDBConnectionString))
            {
                var query = "SELECT COUNT(1) FROM CollaborationData WHERE CollaborationID = @collaborationId AND DatasetID = @datasetId";
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@collaborationId", collaborationId);
                    command.Parameters.AddWithValue("@datasetId", datasetId);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
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
