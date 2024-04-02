
CREATE TABLE Accounts (
	AccountID INT IDENTITY(1,1) PRIMARY KEY,
	UserType SMALLINT,
	UserTypeDescription VARCHAR(30)
);

CREATE TABLE KnowledgeGroups (
	KnowledgeGroupID INT IDENTITY(1,1) PRIMARY KEY,
	Title VARCHAR(255),
	Description TEXT
);

CREATE TABLE Users (
	UserID INT IDENTITY(1, 1) PRIMARY KEY,
	AccountID INT,
	FirstName VARCHAR(100),
	LastName VARCHAR(100),
	EmailAddress NVARCHAR(255),
	PhoneNumber NVARCHAR(255),
	StreetAddress NVARCHAR(255),
	City VARCHAR(50),
	State VARCHAR(50),
	PostalCode INT,
	Country VARCHAR(255),
	FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);

CREATE TABLE KnowledgeGroupParticipants (
	KnowledgeGroupID INT,
	UserID INT,
	PRIMARY KEY (KnowledgeGroupID, UserID),
	FOREIGN KEY (KnowledgeGroupID) REFERENCES KnowledgeGroups(KnowledgeGroupID),
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Datasets (
	DatasetID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT,
	FileName VARCHAR(255),
	DateUploaded DATE,
	Description NVARCHAR(255),
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Dataset_Notes (
	NoteID INT IDENTITY(1,1) PRIMARY KEY,
	DatasetID INT,
	NoteContent NVARCHAR(255),
	FOREIGN KEY (DatasetID) REFERENCES Datasets(DatasetID)
);

CREATE TABLE Chats (
	ChatID INT IDENTITY(1,1) PRIMARY KEY,
	DateCreated DATETIME,
	Title VARCHAR(50)
);

CREATE TABLE Messages (
	MessageID INT IDENTITY(1,1) PRIMARY KEY,
	ChatID INT,
	SentFrom INT,
	MessageContents TEXT,
	DateCreated DATE,
	FOREIGN KEY (ChatID) REFERENCES Chats(ChatID),
	FOREIGN KEY (SentFrom) REFERENCES Users(UserID)
);

CREATE TABLE SwotAnalysis (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Type VARCHAR(50), -- Can be 'Strength', 'Weakness', 'Opportunity', or 'Threat'
    Description VARCHAR(MAX),
    Implications VARCHAR(MAX),
    Strategies VARCHAR(MAX), -- Possible strategies to leverage or mitigate the SWOT factor
    AnalysisDate DATE,
    Notes VARCHAR(MAX)
);

CREATE TABLE PestAnalysis (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Category VARCHAR(50),
    Factor VARCHAR(MAX),
    Implications VARCHAR(MAX),
    PossibleActions VARCHAR(MAX),
    AnalysisDate DATE,
    Notes VARCHAR(MAX)
);

CREATE TABLE Reports (
	ReportID INT IDENTITY(1,1) PRIMARY KEY,
	SwotAnalysisID INT,
	PestAnalysisID INT,
	DateCreated DATETIME,
	FOREIGN KEY (SwotAnalysisID) REFERENCES SwotAnalysis(ID),
	FOREIGN KEY (PestAnalysisID) REFERENCES PestAnalysis(ID)
);

CREATE TABLE KnowledgeItems (
	KnowledgeItemID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT,
	KnowledgeGroupID INT,
	Title NVARCHAR(255),
	Information TEXT,
	DateCreated DATE,
	LastDateModified DATE,
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
	FOREIGN KEY (KnowledgeGroupID) REFERENCES KnowledgeGroups(KnowledgeGroupID)
);

INSERT INTO KnowledgeGroups VALUES 
('Spending Levels & Projections', 'Spending levels and projects for Madison County Government'),
('Administrative Efficiency', 'Measuring Madison County Government administrative efficiency'),
('Personal Policy & Administration', 'Knowledge regarding personal policy and administration'),
('Tax Base Assessment & Projected Revenue', 'Projecting revenue based off of tax based assessments for the next 3-5 years'),
('Economic Development', 'Strategy and implementation regarding economic development'),
('Citizen Communications', 'Knowledge regarding citizen communications and relationships');