CREATE TABLE [dbo].[tblReportColumnsMapping] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ReportId]    INT            NULL,
    [ColumnName]  NVARCHAR (100) NULL,
    [ColumnWidth] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblReportColumnsMapping] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tblReportColumnsMapping_tblReports] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[tblReports] ([Id])
);

