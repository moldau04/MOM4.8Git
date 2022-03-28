CREATE TABLE [dbo].[tblEmailAddresses] (
    [ID]    INT           IDENTITY (1, 1) NOT NULL,
    [Email] VARCHAR (100) NULL, 
    CONSTRAINT [PK_tblEmailAddresses] PRIMARY KEY ([ID])
);

