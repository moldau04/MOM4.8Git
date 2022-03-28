CREATE TABLE [dbo].[IType] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](15) NULL,
	[Count] [int] NULL,
	[Remarks] [varchar](8000) NULL, 
    CONSTRAINT [PK_IType] PRIMARY KEY ([ID])
);

