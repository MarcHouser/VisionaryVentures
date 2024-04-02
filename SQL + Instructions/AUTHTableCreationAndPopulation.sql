CREATE TABLE HashedCredentials (
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	Username NVARCHAR(50),
	PasswordHash NVARCHAR(255)
);

INSERT INTO [Sprint2].[dbo].[Accounts] (UserType, UserTypeDescription) Values (1, 'Administrative');

INSERT INTO [Sprint2].[dbo].[Users]
           ([AccountID]
		   ,[FirstName]
           ,[LastName]
           ,[EmailAddress]
           ,[PhoneNumber]
           ,[StreetAddress]
           ,[City]
           ,[State]
           ,[PostalCode]
           ,[Country])
     VALUES
           (1
		   ,'Admin'
		   ,'User'
		   ,'Admin.User@gmail.com'
		   ,'703626-0156'
		   ,'121 Main St'
		   ,'Harrisonburg'
		   ,'VA'
		   ,22801
		   ,'United States');

INSERT INTO [AUTH].[dbo].[HashedCredentials]
	(Username,
	PasswordHash)
	VALUES ('admin', '1000:Av+wcY5PfhZM7brRKLg7ASbMuowvvB+R:cBmkA0pgUWequQ06sMayq7zeLr8=')