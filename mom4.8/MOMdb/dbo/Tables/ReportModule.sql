CREATE TABLE [dbo].[ReportModule](
	[ReportModuleId] [bigint] IDENTITY(1,1) NOT NULL,
	[ModuleName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[SortOrder] [bigint] NULL,
 CONSTRAINT [PK_ReportModule] PRIMARY KEY CLUSTERED 
(
	[ReportModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO