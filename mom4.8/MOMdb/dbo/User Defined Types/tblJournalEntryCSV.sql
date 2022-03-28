CREATE TYPE [dbo].[tblJournalEntryCSV] AS TABLE(
	[AccNo] [varchar](50) NULL,
	[Memo] [varchar](500) NULL,
	[Amount] [varchar](50) NULL,
	[RowNo] [int] NULL);