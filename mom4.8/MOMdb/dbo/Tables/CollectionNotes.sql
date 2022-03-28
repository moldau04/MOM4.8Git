CREATE TABLE [dbo].[CollectionNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Notes] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[OwnerID] [int] NULL,
    [LocID] INT NULL, 
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [varchar](50) NULL,
    CONSTRAINT [PK_CollectionNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
