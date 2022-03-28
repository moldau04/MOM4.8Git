CREATE TABLE [dbo].[tblCustomReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ReportName] [nvarchar](50) NULL,
	[ReportDesc] [nvarchar](200) NULL,
	[ReportType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblCustomReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
