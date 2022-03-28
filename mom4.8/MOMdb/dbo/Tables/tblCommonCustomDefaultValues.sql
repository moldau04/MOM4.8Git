CREATE TABLE [dbo].[tblCommonCustomDefaultValues] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [tblCommonCustomFieldsID] INT          NULL,
    [Line]              SMALLINT     NULL,
    [Value]             VARCHAR (255) NULL, 
    CONSTRAINT [PK_tblCommonCustomDefaultValues] PRIMARY KEY ([ID])
);

