CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FullName] NCHAR(10) NULL, 
    [OrgName] NCHAR(10) NULL, 
    [ArchiveNo] INT NULL, 
    [OrgTIN] INT NULL, 
    [DocCount] INT NULL, 
    [Allowance] INT NULL, 
    [PhoneNo] INT NULL, 
    [GovIDNo] INT NULL, 
    [PrivateIDNo] INT NULL, 
    [MotherName] NCHAR(10) NULL, 
    [BDate] INT NULL, 
    [DueDate] INT NULL, 
    [DateRecieved] INT NULL, 
    [DateTransfered] INT NULL, 
    [Gender] NCHAR(10) NULL
)
