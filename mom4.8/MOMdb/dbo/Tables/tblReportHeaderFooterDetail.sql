CREATE TABLE [dbo].[tblReportHeaderFooterDetail] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [ReportId]        INT            NULL,
    [MainHeader]      BIT            CONSTRAINT [DF_tblReportHeaderFooterDetail_MainHeader] DEFAULT ((1)) NULL,
    [CompanyName]     NVARCHAR (200) NULL,
    [ReportTitle]     NVARCHAR (200) NULL,
    [SubTitle]        NVARCHAR (200) NULL,
    [DatePrepared]    NVARCHAR (100) NULL,
    [TimePrepared]    BIT            CONSTRAINT [DF_tblReportHeaderFooterDetail_TimePrepared] DEFAULT ((0)) NULL,
    [ReportBasis]     BIT            CONSTRAINT [DF_tblReportHeaderFooterDetail_ReportBasis] DEFAULT ((0)) NULL,
    [PageNumber]      NVARCHAR (50)  NULL,
    [ExtraFooterLine] NVARCHAR (200) NULL,
    [Alignment]       NVARCHAR (50)  NULL,
    [PDFSize]         NVARCHAR (50)  NULL,
    CONSTRAINT [PK_tblReportHeaderFooterDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tblReportHeaderFooterDetail_tblReportHeaderFooterDetail] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[tblReports] ([Id])
);

