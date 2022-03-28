CREATE TABLE [dbo].[tblJoinPhoneJob](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PhoneID] [int] NOT NULL,
	[JobID] [int] NOT NULL, 
    CONSTRAINT [PK_tblJoinPhoneJob] PRIMARY KEY ([ID])
) ON [PRIMARY]

GO
