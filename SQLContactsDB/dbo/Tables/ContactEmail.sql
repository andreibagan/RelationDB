CREATE TABLE [dbo].[ContactEmail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ContactId] int not null,
	[EmailAddressId] int not null
)
