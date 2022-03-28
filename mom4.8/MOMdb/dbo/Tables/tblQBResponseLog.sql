CREATE TABLE [dbo].[tblQBResponseLog] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [API]            VARCHAR (50)  NULL,
    [requestID]      VARCHAR (50)  NULL,
    [StatusCode]     VARCHAR (50)  NULL,
    [statusSeverity] VARCHAR (50)  NULL,
    [statusMessage]  VARCHAR (250) NULL,
    [DateTime]       DATETIME      NULL, 
    CONSTRAINT [PK_tblQBResponseLog] PRIMARY KEY ([ID])
);

