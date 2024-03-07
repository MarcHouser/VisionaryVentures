CREATE TABLE HashedCredentials (
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	Username NVARCHAR(50),
	PasswordHash NVARCHAR(255)
);

INSERT INTO [Lab4].[dbo].[Users]
           ([FirstName]
           ,[LastName]
           ,[EmailAddress]
           ,[PhoneNumber]
           ,[StreetAddress]
           ,[City]
           ,[State]
           ,[PostalCode]
           ,[Country])
     VALUES
           ('Admin'
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