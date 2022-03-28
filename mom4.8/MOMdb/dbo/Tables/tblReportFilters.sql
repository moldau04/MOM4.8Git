CREATE TABLE [dbo].[tblReportFilters] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ReportId]       INT            NULL,
    [FilterColumn]   NVARCHAR (100) NULL,
    [FilterSet]      NVARCHAR (200) NULL,
    [FilterDataType] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblReportFilters] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tblReportFilters_tblReportFilters] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[tblReports] ([Id])
);

