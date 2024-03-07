using System.Data.SqlClient;
using VisionaryVentures.Pages.DataClasses;

namespace VisionaryVentures.Pages.DB
{
    public class DBClassWriters
    {
        // Create SQL connection
        public static SqlConnection LabOneDBConnection = new SqlConnection();
        public static SqlConnection AuthConn = new SqlConnection();

        // Instantiate connection string
        private static readonly String? LabOneDBConnectionString =
            "Server=localhost;Database=Lab4;Trusted_Connection=True";
        private static readonly String? AuthConnString =
            "Server=localhost;Database=AUTH;Trusted_Connection=True";

        // Method to add a user
        public static void AddUser(string firstName, string lastName, string EmailAddress, string phoneNumber,
            string streetAddress, string city, string state, int postalCode, string country)
        {
            string userInsertQuery = "INSERT INTO Users (FirstName, LastName, EmailAddress, PhoneNumber, StreetAddress, City, State, PostalCode, Country) " +
                "VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber, @StreetAddress, @City, @State, @PostalCode, @Country)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                // Insert into Users with the captured AccountID
                using (SqlCommand userInsertCommand = new SqlCommand(userInsertQuery, connection))
                {
                    userInsertCommand.Parameters.AddWithValue("@FirstName", firstName);
                    userInsertCommand.Parameters.AddWithValue("@LastName", lastName);
                    userInsertCommand.Parameters.AddWithValue("@EmailAddress", EmailAddress);
                    userInsertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    userInsertCommand.Parameters.AddWithValue("@StreetAddress", streetAddress);
                    userInsertCommand.Parameters.AddWithValue("@City", city);
                    userInsertCommand.Parameters.AddWithValue("@State", state);
                    userInsertCommand.Parameters.AddWithValue("@PostalCode", postalCode);
                    userInsertCommand.Parameters.AddWithValue("@Country", country);

                    userInsertCommand.ExecuteNonQuery();
                }
            }
        }

        public static void CreateHashedUser(string Username, string Password)
        {
            string loginQuery =
                "INSERT INTO HashedCredentials (Username,PasswordHash) values (@Username, @Password)";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = LabOneDBConnection;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);
            cmdLogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));

            cmdLogin.Connection.Open();

            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            cmdLogin.ExecuteNonQuery();

        }

        // Method to build a collaboration
        public static void AddCollaboration(String Title, String Description)
        {

            String CollabInsertQuery = "INSERT INTO Collaborations VALUES (@Title, @Description)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand CollabInsertCommand = new SqlCommand(CollabInsertQuery, connection))
                {
                    CollabInsertCommand.Parameters.AddWithValue("@Title", Title);
                    CollabInsertCommand.Parameters.AddWithValue("@Description", Description);

                    CollabInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Method to add a user to a collaboration
        public static void AddUserToCollaboration(int UserID, int CollabID)
        {

            String UserCollabInsertQuery = "INSERT INTO CollaborationParticipants VALUES (@CollabID, @UserID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand UserCollabInsertCommand = new SqlCommand(UserCollabInsertQuery, connection))
                {
                    UserCollabInsertCommand.Parameters.AddWithValue("@CollabID", CollabID);
                    UserCollabInsertCommand.Parameters.AddWithValue("@UserID", UserID);

                    UserCollabInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Method to add a dataset
        public static void AddDataset(int UserID, String FileName, DateTime DateUploaded)
        {

            String DatasetInsertQuery = "INSERT INTO Datasets VALUES (@UserID, @FileName, @DateUploaded)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand DatasetInsertCommand = new SqlCommand(DatasetInsertQuery, connection))
                {
                    DatasetInsertCommand.Parameters.AddWithValue("@UserID", UserID);
                    DatasetInsertCommand.Parameters.AddWithValue("@FileName", FileName);
                    DatasetInsertCommand.Parameters.AddWithValue("@DateUploaded", DateUploaded);

                    DatasetInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Method to add a dataset to a collaboration
        public static void AddDatasetToCollaboration(int CollabID, int DatasetID)
        {

            String DatasetCollabInsertQuery = "INSERT INTO CollaborationData VALUES (@CollabID, @DatasetID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand DatasetCollabInsertCommand = new SqlCommand(DatasetCollabInsertQuery, connection))
                {
                    DatasetCollabInsertCommand.Parameters.AddWithValue("@CollabID", CollabID);
                    DatasetCollabInsertCommand.Parameters.AddWithValue("@DatasetID", DatasetID);

                    DatasetCollabInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Method to add an analysis to dataset
        public static void AddAnalysisToDataset(int DatasetID, String AnalysisType)
        {

            String AnalysisDatasetInsertQuery = "INSERT INTO Analyses VALUES (@DatasetID, @AnalysisType)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand AnalysisDatasetInsertCommand = new SqlCommand(AnalysisDatasetInsertQuery, connection))
                {
                    AnalysisDatasetInsertCommand.Parameters.AddWithValue("@DatasetID", DatasetID);
                    AnalysisDatasetInsertCommand.Parameters.AddWithValue("@AnalysisType", AnalysisType);

                    AnalysisDatasetInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Add Knowledge Item
        public static void AddKnowledgeItem(int UserID, String Category, String Title, String Information, DateTime DateCreated, DateTime LastDateModified)
        {

            String KnowledgeItemInsertQuery = "INSERT INTO KnowledgeItems VALUES (@UserID, @Category, @Title, @Information, @DateCreated, @LastDateModified)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand KnowledgeItemInsertCommand = new SqlCommand(KnowledgeItemInsertQuery, connection))
                {
                    KnowledgeItemInsertCommand.Parameters.AddWithValue("@UserID", UserID);
                    KnowledgeItemInsertCommand.Parameters.AddWithValue("@Category", Category);
                    KnowledgeItemInsertCommand.Parameters.AddWithValue("@Title", Title);
                    KnowledgeItemInsertCommand.Parameters.AddWithValue("@Information", Information);
                    KnowledgeItemInsertCommand.Parameters.AddWithValue("@DateCreated", DateCreated);
                    KnowledgeItemInsertCommand.Parameters.AddWithValue("@LastDateModified", LastDateModified);

                    KnowledgeItemInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Edit Knowledge Item
        public static void EditKnowledgeItem(int KnowledgeItemID, String Information, DateTime LastDateModified)
        {
            String KnowledgeItemEditQuery = "UPDATE KnowledgeItems SET Information = @Information, LastDateModified = @LastDateModified " +
                "WHERE KnowledgeItemID = @KnowledgeItemID";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand KnowledgeItemEditCommand = new SqlCommand(KnowledgeItemEditQuery, connection))
                {
                    KnowledgeItemEditCommand.Parameters.AddWithValue("@KnowledgeItemID", KnowledgeItemID);
                    KnowledgeItemEditCommand.Parameters.AddWithValue("@Information", Information);
                    KnowledgeItemEditCommand.Parameters.AddWithValue("@LastDateModified", LastDateModified);

                    KnowledgeItemEditCommand.ExecuteNonQuery();
                }
            }
        }

        // Add Knowledge Item to Collaboration
        public static void AddKnowledgeItemToCollaboration(int KnowledgeItemID, int CollaborationID)
        {

            String KnowledgeItemCollabInsertQuery = "INSERT INTO CollaborationKnowledge VALUES (@KnowledgeItemID, @CollaborationID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand KnowledgeItemCollabInsertCommand = new SqlCommand(KnowledgeItemCollabInsertQuery, connection))
                {
                    KnowledgeItemCollabInsertCommand.Parameters.AddWithValue("@KnowledgeItemID", KnowledgeItemID);
                    KnowledgeItemCollabInsertCommand.Parameters.AddWithValue("@CollaborationID", CollaborationID);

                    KnowledgeItemCollabInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Add Message
        public static void AddMessage(int CollaborationID, int UserID, String MessageContents, DateTime DateCreated)
        {

            String MessageInsertQuery = "INSERT INTO Messages VALUES (@CollaborationID, @UserID, @MessageContents, @DateSent)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand MessageInsertCommand = new SqlCommand(MessageInsertQuery, connection))
                {
                    MessageInsertCommand.Parameters.AddWithValue("@CollaborationID", CollaborationID);
                    MessageInsertCommand.Parameters.AddWithValue("@UserID", UserID);
                    MessageInsertCommand.Parameters.AddWithValue("@MessageContents", MessageContents);
                    MessageInsertCommand.Parameters.AddWithValue("@DateSent", DateCreated);

                    MessageInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Add Plan
        public static void AddPlan(String PlanName, String PlanDescription, DateTime DateCreated)
        {
            String PlanInsertQuery = "INSERT INTO Plans VALUES (@PlanName, @PlanDescription, @DateCreated)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand PlanInsertCommand = new SqlCommand(PlanInsertQuery, connection))
                {
                    PlanInsertCommand.Parameters.AddWithValue("@PlanName", PlanName);
                    PlanInsertCommand.Parameters.AddWithValue("@PlanDescription", PlanDescription);
                    PlanInsertCommand.Parameters.AddWithValue("@DateCreated", DateCreated);

                    PlanInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // User plan correlation
        public static void AddUserPlan(int UserID, int PlanID)
        {
            String UserPlanInsertQuery = "INSERT INTO UserPlans VALUES (@UserID, @PlanID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand UserPlanInsertCommand = new SqlCommand(UserPlanInsertQuery, connection))
                {
                    UserPlanInsertCommand.Parameters.AddWithValue("@UserID", UserID);
                    UserPlanInsertCommand.Parameters.AddWithValue("@PlanID", PlanID);

                    UserPlanInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Add Plan Contents
        public static void AddPlanContents(int PlanID, int PlanStep, String Description)
        {
            String PlanContentsInsertQuery = "INSERT INTO PlanContents VALUES (@PlanID, @PlanStep, @Description)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand PlanContentsInsertCommand = new SqlCommand(PlanContentsInsertQuery, connection))
                {
                    PlanContentsInsertCommand.Parameters.AddWithValue("@PlanID", PlanID);
                    PlanContentsInsertCommand.Parameters.AddWithValue("@PlanStep", PlanStep);
                    PlanContentsInsertCommand.Parameters.AddWithValue("@Description", Description);

                    PlanContentsInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Add Plan to Collaboration
        public static void AddPlanToCollaboration(int CollaborationID, int PlanID)
        {
            String PlanCollabInsertQuery = "INSERT INTO CollaborationPlans VALUES (@CollaborationID, @PlanID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand PlanCollabInsertCommand = new SqlCommand(PlanCollabInsertQuery, connection))
                {
                    PlanCollabInsertCommand.Parameters.AddWithValue("@CollaborationID", CollaborationID);
                    PlanCollabInsertCommand.Parameters.AddWithValue("@PlanID", PlanID);

                    PlanCollabInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Add SWOT
        public static void UpsertSwotAnalysis(int knowledgeItemID, string strengths, string weaknesses, string opportunities, string threats)
        {
            // Define the connection outside the try block to ensure it's in scope for the finally block
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(LabOneDBConnectionString);
                connection.Open();

                // Define an upsert command. The usage of IF EXISTS...ELSE simplifies the control flow for the upsert operation
                string upsertQuery = @"
            IF EXISTS (SELECT 1 FROM SwotAnalyses WHERE KnowledgeItemID = @KnowledgeItemID)
            BEGIN
                UPDATE SwotAnalyses
                SET Strengths = @Strengths, Weaknesses = @Weaknesses, Opportunities = @Opportunities, Threats = @Threats
                WHERE KnowledgeItemID = @KnowledgeItemID
            END
            ELSE
            BEGIN
                INSERT INTO SwotAnalyses (KnowledgeItemID, Strengths, Weaknesses, Opportunities, Threats)
                VALUES (@KnowledgeItemID, @Strengths, @Weaknesses, @Opportunities, @Threats)
            END";

                using (SqlCommand command = new SqlCommand(upsertQuery, connection))
                {
                    command.Parameters.AddWithValue("@KnowledgeItemID", knowledgeItemID);
                    command.Parameters.AddWithValue("@Strengths", strengths ?? string.Empty); // Handle potential null values
                    command.Parameters.AddWithValue("@Weaknesses", weaknesses ?? string.Empty);
                    command.Parameters.AddWithValue("@Opportunities", opportunities ?? string.Empty);
                    command.Parameters.AddWithValue("@Threats", threats ?? string.Empty);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log exception (adjust logging mechanism based on your application's logging framework)
                Console.Error.WriteLine($"An error occurred in UpsertSwotAnalysis: {ex.Message}");
            }
            finally
            {
                // Ensure the connection is closed even if an exception occurs
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

    }
}