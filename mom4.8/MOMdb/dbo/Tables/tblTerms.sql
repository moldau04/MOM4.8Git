CREATE TABLE [dbo].[tblTerms] (
    [ID]             INT           IDENTITY (0, 1) NOT NULL,
    [Name]           VARCHAR (50)  NULL,
    [QBTermsID]      VARCHAR (100) NULL,
    [LastUpdateDate] DATETIME      NULL, 
    CONSTRAINT [PK_tblTerms] PRIMARY KEY ([ID])
);

