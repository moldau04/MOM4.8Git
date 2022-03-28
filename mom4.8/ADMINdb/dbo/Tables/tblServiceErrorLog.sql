CREATE TABLE [dbo].[tblServiceErrorLog] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ServiceName] VARCHAR (25)   NULL,
    [Error]       VARCHAR (1000) NULL,
    [Date]        DATETIME       CONSTRAINT [DF_tblServiceErrorLog_Date] DEFAULT (getdate()) NULL
);

