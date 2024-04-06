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
            "Initial Catalog=Sprint3;" +
            "Initial Catalog=AUTH;" +
            "Persist Security Info=False;" +
            "User ID=VisionaryVenturesAdmin;" +
            "Password=COB484Capstone;" +
            "MultipleActiveResultSets=False;" +
            "Encrypt=True;" +
            "TrustServerCertificate=False;" +
            "Connection Timeout=30;";

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

        // Create a hashed user
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

        // Add Chat
        public static void AddChat(string Title, DateTime DateCreated)
        {
            String ChatInsertQuery = "INSERT INTO Chats VALUES (@DateCreated, @Title)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand ChatInsertCommand = new SqlCommand(ChatInsertQuery, connection))
                {
                    ChatInsertCommand.Parameters.AddWithValue("@DateCreated", DateCreated);
                    ChatInsertCommand.Parameters.AddWithValue("@Title", Title);

                    ChatInsertCommand.ExecuteNonQuery();
                }
            }
        }

        

        // Add SWOT
        public static int InsertSWOTAnalysis(string implications, string strategies, DateTime analysisDate, string notes, int KnowledgeGroupID,
            string Strengths, string Weaknesses, string Opportunities, string Threats)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string insertQuery = @"
            INSERT INTO SwotAnalysis(Implications, Strategies, AnalysisDate, Notes, KnowledgeGroupID, Strengths, Weaknesses, Opportunities, Threats)
            VALUES (@Implications, @Strategies, @AnalysisDate, @Notes, @KnowledgeGroupID, @Strengths, @Weaknesses, @Opportunities, @Threats);
            SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Implications", implications);
                command.Parameters.AddWithValue("@Strategies", strategies);
                command.Parameters.AddWithValue("@AnalysisDate", analysisDate);
                command.Parameters.AddWithValue("@Notes", notes);
                command.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);
                command.Parameters.AddWithValue("@Strengths", Strengths);
                command.Parameters.AddWithValue("@Weaknesses", Weaknesses);
                command.Parameters.AddWithValue("@Opportunities", Opportunities);
                command.Parameters.AddWithValue("@Threats", Threats);

                connection.Open();
                int swotId = Convert.ToInt32(command.ExecuteScalar());
                return swotId;
            }
        }

        // Add PEST
        public static int InsertPESTAnalysis(string category, string factor, string implications, string possibleActions, DateTime analysisDate, string notes, int KnowledgeGroupID)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string insertQuery = @"
            INSERT INTO PestAnalysis(Category, Factor, Implications, PossibleActions, AnalysisDate, Notes, KnowledgeGroupID)
            VALUES (@Category, @Factor, @Implications, @PossibleActions, @AnalysisDate, @Notes, @KnowledgeGroupID);
            SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Category", category);
                command.Parameters.AddWithValue("@Factor", factor);
                command.Parameters.AddWithValue("@Implications", implications);
                command.Parameters.AddWithValue("@PossibleActions", possibleActions);
                command.Parameters.AddWithValue("@AnalysisDate", analysisDate);
                command.Parameters.AddWithValue("@Notes", notes);
                command.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);

                connection.Open();
                int pestId = Convert.ToInt32(command.ExecuteScalar());
                return pestId;
            }
        }

        // Add Report
        public static int CreateReport(int swotAnalysisId, int pestAnalysisId, string title, string description)
        {
            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                string insertQuery = @"
            INSERT INTO Reports(SwotAnalysisID, PestAnalysisID, DateCreated, Title, Description)
            VALUES (@SwotAnalysisID, @PestAnalysisID, GETDATE(), @Title, @Description);
            SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@SwotAnalysisID", swotAnalysisId);
                command.Parameters.AddWithValue("@PestAnalysisID", pestAnalysisId);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);

                connection.Open();
                int reportId = Convert.ToInt32(command.ExecuteScalar());
                return reportId;
            }
        }

        // Build Report with SWOT and PEST analysis
        public static void BuildReport(DateTime ReportDateCreated, string ReportTitle, string ReportDescription, string SwotImplications, string SwotStrategies, DateTime SwotDateCreated,
            string SwotNotes, int KnowledgeGroupID, string SwotStrengths, string SwotWeaknesses, string SwotOpportunities, string SwotThreats, 
            string PestCategory, string PestFactor, string PestImplications, string PestPossibleActions, DateTime PestDateCreated, string PestNotes)
        {
            string ReportInsertQuery = "INSERT INTO Reports (DateCreated, Title, Description, KnowledgeGroupID) OUTPUT INSERTED.ReportID " +
                "VALUES (@ReportDateCreated, @ReportTitle, @ReportDescription, @KnowledgeGroupID)";
            string SwotInsertQuery = "INSERT INTO SwotAnalysis VALUES (@SwotImplications, @SwotStrategies, @SwotDateCreated, @SwotNotes, @KnowledgeGroupID, " +
                "@SwotStrengths, @SwotWeaknesses, @SwotOpportunities, @SwotThreats, @ReportID)";
            string PestInsertQuery = "INSERT INTO PestAnalysis VALUES (@PestCategory, @PestFactor, @PestImplications, @PestPossibleActions, " +
                "@PestDateCreated, @PestNotes, @KnowledgeGroupID, @ReportID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert into Reports and retrieve the new ReportID
                    using (SqlCommand ReportInsertCommand = new SqlCommand(ReportInsertQuery, connection, transaction))
                    {
                        ReportInsertCommand.Parameters.AddWithValue("@ReportDateCreated", ReportDateCreated);
                        ReportInsertCommand.Parameters.AddWithValue("@ReportTitle", ReportTitle);
                        ReportInsertCommand.Parameters.AddWithValue("@ReportDescription", ReportDescription);
                        ReportInsertCommand.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);

                        int reportID = (int)ReportInsertCommand.ExecuteScalar();

                        // Now insert into SwotAnalyses with the captured ReportID
                        using (SqlCommand SwotInsertCommand = new SqlCommand(SwotInsertQuery, connection, transaction))
                        {
                            SwotInsertCommand.Parameters.AddWithValue("@SwotImplications", SwotImplications);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotStrategies", SwotStrategies);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotDateCreated", SwotDateCreated);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotNotes", SwotNotes);
                            SwotInsertCommand.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotStrengths", SwotStrengths);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotWeaknesses", SwotWeaknesses);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotOpportunities", SwotOpportunities);
                            SwotInsertCommand.Parameters.AddWithValue("@SwotThreats", SwotThreats);
                            SwotInsertCommand.Parameters.AddWithValue("@ReportID", reportID);

                            SwotInsertCommand.ExecuteNonQuery();
                        }

                        // Now insert into PestAnalyses with the captured ReportID
                        using (SqlCommand PestInsertCommand = new SqlCommand(PestInsertQuery, connection, transaction))
                        {
                            PestInsertCommand.Parameters.AddWithValue("@PestCategory", PestCategory);
                            PestInsertCommand.Parameters.AddWithValue("@PestFactor", PestFactor);
                            PestInsertCommand.Parameters.AddWithValue("@PestImplications", PestImplications);
                            PestInsertCommand.Parameters.AddWithValue("@PestPossibleActions", PestPossibleActions);
                            PestInsertCommand.Parameters.AddWithValue("@PestDateCreated", PestDateCreated);
                            PestInsertCommand.Parameters.AddWithValue("@PestNotes", PestNotes);
                            PestInsertCommand.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);
                            PestInsertCommand.Parameters.AddWithValue("@ReportID", reportID);

                            PestInsertCommand.ExecuteNonQuery();
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

        // Method to create a new knowledge group
        public static void CreateKnowledgeGroup(String GroupName, String Description)
        {

            String KnowledgeGroupInsertQuery = "INSERT INTO KnowledgeGroups VALUES (@GroupName, @Description)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand KnowledgeGroupInsertCommand = new SqlCommand(KnowledgeGroupInsertQuery, connection))
                {
                    KnowledgeGroupInsertCommand.Parameters.AddWithValue("@GroupName", GroupName);
                    KnowledgeGroupInsertCommand.Parameters.AddWithValue("@Description", Description);

                    KnowledgeGroupInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Method to add a user to a knowledge group
        public static void AddUserToKnowledgeGroup(int UserID, int KnowledgeGroupID)
        {

            String UserCollabInsertQuery = "INSERT INTO KnowledgeGroupParticipants VALUES (@KnowledgeGroupID, @UserID)";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand UserCollabInsertCommand = new SqlCommand(UserCollabInsertQuery, connection))
                {
                    UserCollabInsertCommand.Parameters.AddWithValue("@KnowledgeGroupID", KnowledgeGroupID);
                    UserCollabInsertCommand.Parameters.AddWithValue("@UserID", UserID);

                    UserCollabInsertCommand.ExecuteNonQuery();
                }
            }
        }

        // Update description of dataset
        public static void UpdateDatasetDescription(string FileName, string Description)
        {
            String DatasetUpdateQuery = "UPDATE Datasets SET Description = @Description WHERE FileName = @FileName";

            using (SqlConnection connection = new SqlConnection(LabOneDBConnectionString))
            {
                connection.Open();

                using (SqlCommand DatasetUpdateCommand = new SqlCommand(DatasetUpdateQuery, connection))
                {
                    DatasetUpdateCommand.Parameters.AddWithValue("@FileName", FileName);
                    DatasetUpdateCommand.Parameters.AddWithValue("@Description", Description);

                    DatasetUpdateCommand.ExecuteNonQuery();
                }
            }
        }
    }
}