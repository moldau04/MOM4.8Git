CREATE TABLE [dbo].[T&C] (
    [ID]              INT            IDENTITY (1, 1) NOT NULL,
    [tblPageID]       INT            NULL,
    [TermsConditions] VARCHAR (5000) NULL,
    CONSTRAINT [PK_T&C] PRIMARY KEY CLUSTERED ([ID] ASC)
);

