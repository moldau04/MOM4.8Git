CREATE TABLE [dbo].[tblReports] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ReportName]       NVARCHAR (200) NULL,
    [ReportType]       NVARCHAR (200) NULL,
    [UserId]           INT            NULL,
    [IsGlobal]         BIT            CONSTRAINT [DF_tblReports_IsGlobal] DEFAULT ((0)) NULL,
    [IsAscendingOrder] BIT            CONSTRAINT [DF_tblReports_IsAscendingOrder] DEFAULT ((1)) NULL,
    [SortBy]           NVARCHAR (200) NULL,
    [IsStock]          BIT            NULL,
    [Module]           NVARCHAR (MAX) NULL,
    [Condition] NVARCHAR(500) NULL, 
    CONSTRAINT [PK_tblReports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_tblReports_tblReports] FOREIGN KEY ([Id]) REFERENCES [dbo].[tblReports] ([Id])
);

