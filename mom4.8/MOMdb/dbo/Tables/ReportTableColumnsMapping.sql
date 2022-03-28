
CREATE TABLE [dbo].[ReportTableColumnsMapping](
	[ReportTableColumnId] [bigint] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](50) NULL,
	[DBColumnName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[ReportTableId] [bigint] NULL,
	[SortOrder] [bigint] NULL,
 CONSTRAINT [PK_ReportTableColumnsMapping] PRIMARY KEY CLUSTERED 
(
	[ReportTableColumnId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


