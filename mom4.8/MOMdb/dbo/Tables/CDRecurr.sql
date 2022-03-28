CREATE TABLE [dbo].[CDRecurr](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[Ref] BIGINT NULL,
	[fDesc] [varchar](250) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Bank] [int] NULL,
	[Type] [smallint] NULL,
	[Status] [smallint] NULL,
	[TransID] [int] NULL,
	[Vendor] [int] NULL,
	[French] [varchar](255) NULL,
	[Memo] [varchar](75) NULL,
	[VoidR] [varchar](75) NULL,
	[ACH] [tinyint] NULL,
	[IsRecon] [bit] NULL,
	[Frequency] [int] NULL,
	[PJID] [int] NULL
) ON [PRIMARY]
