CREATE TABLE [dbo].[ElevShutdownReason]
(
	[ID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Reason] VARCHAR(100) NOT NULL, 
    [Planned] BIT NOT NULL DEFAULT 0,
	[CreatedDate] Datetime,
	[CreatedBy] VARCHAR(100),
	[UpdatedDate] Datetime,
	[UpdatedBy] VARCHAR(100) 
)
