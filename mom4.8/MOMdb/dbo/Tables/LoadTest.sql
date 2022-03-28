CREATE TABLE [dbo].[LoadTest](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[Authority] [varchar](25) NULL,
	[Frequency] [smallint] NULL,
	[Remarks] [varchar](8000) NULL,
	[Count] [smallint] NULL,
	[Level] [smallint] NULL,
	[Cat] [varchar](25) NULL,
	[fDesc] [varchar](1000) NULL,
	[NextDateCalcMode] [tinyint] NOT NULL, 
    [Charge] SMALLINT NULL, 
    [ThirdParty] SMALLINT NULL,
	[Status] SMALLINT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LoadTest] ADD  CONSTRAINT [DF_LoadTest_NextDateCalcMode]  DEFAULT ((0)) FOR [NextDateCalcMode]
GO