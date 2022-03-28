
CREATE TABLE [dbo].[ReportTable](
	[ReportTableID] [bigint] IDENTITY(1,1) NOT NULL,
	[DBTableName] [nvarchar](50) NULL,
	[TableName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[SortOrder] [bigint] NULL,
	[ReportModuleId] [bigint] NULL,
	[ParentTableId] [nchar](10) NULL,
 CONSTRAINT [PK_ReportTable] PRIMARY KEY CLUSTERED 
(
	[ReportTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO