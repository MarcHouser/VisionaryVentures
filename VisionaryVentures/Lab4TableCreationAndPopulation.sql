CREATE TABLE Collaborations (
	CollaborationID INT IDENTITY(1,1) PRIMARY KEY,
	Title VARCHAR(255),
	Description TEXT
);

CREATE TABLE Users (
	UserID INT IDENTITY(1, 1) PRIMARY KEY,
	FirstName VARCHAR(100),
	LastName VARCHAR(100),
	EmailAddress NVARCHAR(255),
	PhoneNumber NVARCHAR(255),
	StreetAddress NVARCHAR(255),
	City VARCHAR(50),
	State VARCHAR(50),
	PostalCode INT,
	Country VARCHAR(255),
);

CREATE TABLE Plans (
	PlanID INT IDENTITY(1, 1) PRIMARY KEY,
	PlanName NVARCHAR(100),
	PlanDescription VARCHAR(255),
	DateCreated DATE
);

CREATE TABLE PlanContents (
	PlanContentsID INT IDENTITY(1,1) PRIMARY KEY,
	PlanID INT,
	PlanStep INT,
	ContentDescription TEXT,
	FOREIGN KEY (PlanID) REFERENCES Plans(PlanID)
);

CREATE TABLE UserPlans (
	UserID INT,
	PlanID INT,
	PRIMARY KEY (UserID, PlanID),
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
	FOREIGN KEY (PlanID) REFERENCES Plans(PlanID)
);

CREATE TABLE CollaborationPlans (
	CollaborationID INT,
	PlanID INT,
	PRIMARY KEY (CollaborationID, PlanID),
	FOREIGN KEY (CollaborationID) REFERENCES Collaborations(CollaborationID),
	FOREIGN KEY (PlanID) REFERENCES Plans(PlanID)
);

CREATE TABLE KnowledgeItems (
	KnowledgeItemID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT,
	Category VARCHAR(255),
	Title NVARCHAR(255),
	Information TEXT,
	DateCreated DATE,
	LastDateModified DATE,
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE CollaborationKnowledge (
	KnowledgeItemID INT,
	CollaborationID INT,
	PRIMARY KEY (KnowledgeItemID, CollaborationID),
	FOREIGN KEY (KnowledgeItemID) REFERENCES KnowledgeItems(KnowledgeItemID),
	FOREIGN KEY (CollaborationID) REFERENCES Collaborations(CollaborationID)
);

CREATE TABLE CollaborationParticipants (
	CollaborationID INT,
	UserID INT,
	PRIMARY KEY (CollaborationID, UserID),
	FOREIGN KEY (CollaborationID) REFERENCES Collaborations(CollaborationID),
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Datasets (
	DatasetID INT IDENTITY(1,1) PRIMARY KEY,
	UserID INT,
	FileName VARCHAR(255),
	DateUploaded DATE,
	FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE CollaborationData (
	CollaborationID INT,
	DatasetID INT,
	PRIMARY KEY (CollaborationID, DatasetID),
	FOREIGN KEY (CollaborationID) REFERENCES Collaborations(CollaborationID),
	FOREIGN KEY (DatasetID) REFERENCES Datasets(DatasetID)
);

CREATE TABLE Messages (
	MessageID INT IDENTITY(1,1) PRIMARY KEY,
	CollaborationID INT,
	SentFrom INT,
	MessageContents TEXT,
	DateCreated DATE,
	FOREIGN KEY (CollaborationID) REFERENCES Collaborations(CollaborationID),
	FOREIGN KEY (SentFrom) REFERENCES Users(UserID)
);

CREATE TABLE SwotAnalyses (
    SwotAnalysisID INT IDENTITY(1,1) PRIMARY KEY,
    KnowledgeItemID INT UNIQUE,
    Strengths TEXT,
    Weaknesses TEXT,
    Opportunities TEXT,
    Threats TEXT,
    FOREIGN KEY (KnowledgeItemID) REFERENCES KnowledgeItems(KnowledgeItemID)
);
INSERT INTO Users VALUES ('admin', 'password', 'admin.user@gmail.com', '70364628319', '323 Main st', 'Richmond', 'va', 23233, 'United States');

INSERT INTO Collaborations (Title, Description) VALUES ('JMU COB Network Analysis', 'Analyzing the As-Is Model of Hartman Hall and Zane Showker Hall.');
INSERT INTO Collaborations (Title, Description) VALUES ('Precision Analytica', 'Focused on finding solutions derived from analytical research.');
INSERT INTO Collaborations (Title, Description) VALUES ('Business Plan Collab', 'COB 300 Business Plan Collaboration.');
INSERT INTO Collaborations (Title, Description) VALUES ('Biology Studies', 'Delving into the world of Biology.');

INSERT INTO Plans (PlanName, PlanDescription, DateCreated) VALUES ('COB 300 Operations Plan', 'Designing the infrastructure of the operations deliverables for COB 300', '2024-01-01');
INSERT INTO Plans (PlanName, PlanDescription, DateCreated) VALUES ('JMU COB Network Analysis Research Phase', 'Analyzing the AS-IS model of the network infrastructure of Hartman Hall', '2024-01-05');

INSERT INTO PlanContents (PlanID, PlanStep, ContentDescription) VALUES (1, 1, 'Design Flow Diagram.');
INSERT INTO PlanContents (PlanID, PlanStep, ContentDescription) VALUES (1, 2, 'Decide upon QA methodology.');
INSERT INTO PlanContents (PlanID, PlanStep, ContentDescription) VALUES (1, 3, 'Determine bottlenecks in business process.');
INSERT INTO PlanContents (PlanID, PlanStep, ContentDescription) VALUES (2, 1, 'Contact network admin of Hartman Hall.');
INSERT INTO PlanContents (PlanID, PlanStep, ContentDescription) VALUES (2, 2, 'Visit Hartman Hall.');

INSERT INTO CollaborationPlans (CollaborationID, PlanID) VALUES (1, 1);
INSERT INTO CollaborationPlans (CollaborationID, PlanID) VALUES (2, 2);