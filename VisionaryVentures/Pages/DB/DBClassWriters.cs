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
        public static void AddUserWithAccount(short userType, string userTypeDescription, string firstName, string lastName, string emailAddress, string phoneNumber, string streetAddress, string city, string state, int postalCode, string country)
        {
            string accountInsertQuery = "INSERT INTO Accounts (UserType, UserTypeDescription) OUTPUT INSERTED.AccountID VALUES (@UserType, @UserTypeDescription);";
            string userInsertQuery = "INSERT INTO Users (AccountID, FirstName, LastName, EmailAddress, PhoneNumber, StreetAddress, City, State, PostalCode, Country) VALUES (@AccountID, @FirstName, @LastName, @EmailAddress, @PhoneNumber, @StreetAddress, @City, @State, @PostalCode, @Country);";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert into Accounts and retrieve the new AccountID
                    using (SqlCommand accountInsertCommand = new SqlCommand(accountInsertQuery, connection, transaction))
                    {
                        accountInsertCommand.Parameters.AddWithValue("@UserType", userType);
                        accountInsertCommand.Parameters.AddWithValue("@UserTypeDescription", userTypeDescription);

                        int accountID = (int)accountInsertCommand.ExecuteScalar();

                        // Now insert into Users with the captured AccountID
                        using (SqlCommand userInsertCommand = new SqlCommand(userInsertQuery, connection, transaction))
                        {
                            userInsertCommand.Parameters.AddWithValue("@AccountID", accountID);
                            userInsertCommand.Parameters.AddWithValue("@FirstName", firstName);
                            userInsertCommand.Parameters.AddWithValue("@LastName", lastName);
                            userInsertCommand.Parameters.AddWithValue("@EmailAddress", emailAddress);
                            userInsertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            userInsertCommand.Parameters.AddWithValue("@StreetAddress", streetAddress);
                            userInsertCommand.Parameters.AddWithValue("@City", city);
                            userInsertCommand.Parameters.AddWithValue("@State", state);
                            userInsertCommand.Parameters.AddWithValue("@PostalCode", postalCode);
                            userInsertCommand.Parameters.AddWithValue("@Country", country);

                            userInsertCommand.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; // Rethrow the exception to handle it outside or log it
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

        // Method to add a dataset
        public static void AddDataset(int UserID, String FileName, DateTime DateUploaded, string Description)
        {

            String DatasetInsertQuery = "INSERT INTO Datasets VALUES (@UserID, @FileName, @DateUploaded, @Description)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand DatasetInsertCommand = new SqlCommand(DatasetInsertQuery, connection))
                {
                    DatasetInsertCommand.Parameters.AddWithValue("@UserID", UserID);
                    DatasetInsertCommand.Parameters.AddWithValue("@FileName", FileName);
                    DatasetInsertCommand.Parameters.AddWithValue("@DateUploaded", DateUploaded);
                    DatasetInsertCommand.Parameters.AddWithValue("@Description", Description);

                    DatasetInsertCommand.ExecuteNonQuery();
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

        // Add Message
        public static void AddMessage(int ChatID, int UserID, String MessageContents, DateTime DateCreated)
        {

            String MessageInsertQuery = "INSERT INTO Messages VALUES (@ChatID, @UserID, @MessageContents, @DateSent)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand MessageInsertCommand = new SqlCommand(MessageInsertQuery, connection))
                {
                    MessageInsertCommand.Parameters.AddWithValue("@ChatID", ChatID);
                    MessageInsertCommand.Parameters.AddWithValue("@UserID", UserID);
                    MessageInsertCommand.Parameters.AddWithValue("@MessageContents", MessageContents);
                    MessageInsertCommand.Parameters.AddWithValue("@DateSent", DateCreated);

                    MessageInsertCommand.ExecuteNonQuery();
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

        public static int InsertSWOTAnalysis(string type, string description, string implications, string strategies, DateTime analysisDate, string notes)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string insertQuery = @"
            INSERT INTO SwotAnalysis(Type, Description, Implications, Strategies, AnalysisDate, Notes)
            VALUES (@Type, @Description, @Implications, @Strategies, @AnalysisDate, @Notes);
            SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Implications", implications);
                command.Parameters.AddWithValue("@Strategies", strategies);
                command.Parameters.AddWithValue("@AnalysisDate", analysisDate);
                command.Parameters.AddWithValue("@Notes", notes);

                connection.Open();
                int swotId = Convert.ToInt32(command.ExecuteScalar());
                return swotId;
            }
        }

        public static int InsertPESTAnalysis(string category, string factor, string implications, string possibleActions, DateTime analysisDate, string notes)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string insertQuery = @"
            INSERT INTO PestAnalysis(Category, Factor, Implications, PossibleActions, AnalysisDate, Notes)
            VALUES (@Category, @Factor, @Implications, @PossibleActions, @AnalysisDate, @Notes);
            SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Factor", factor);
                command.Parameters.AddWithValue("@Implications", implications);
                command.Parameters.AddWithValue("@PossibleActions", possibleActions);
                command.Parameters.AddWithValue("@AnalysisDate", analysisDate);
                command.Parameters.AddWithValue("@Notes", notes);

                connection.Open();
                int pestId = Convert.ToInt32(command.ExecuteScalar());
                return pestId;
            }
        }

        public static int CreateReport(int swotAnalysisId, int pestAnalysisId)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string insertQuery = @"
            INSERT INTO Reports(SwotAnalysisID, PestAnalysisID, DateCreated)
            VALUES (@SwotAnalysisID, @PestAnalysisID, GETDATE());
            SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@SwotAnalysisID", swotAnalysisId);
                command.Parameters.AddWithValue("@PestAnalysisID", pestAnalysisId);

                connection.Open();
                int reportId = Convert.ToInt32(command.ExecuteScalar());
                return reportId;
            }
        }
    }
}