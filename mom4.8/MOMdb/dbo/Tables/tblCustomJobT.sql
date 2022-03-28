CREATE TABLE [dbo].[tblCustomJobT] (
    [JobTID]            INT          NULL,
    [tblCustomFieldsID] INT          NULL,
    [Value]             VARCHAR (50) NULL,
    [JobID]             INT          NULL,
    CONSTRAINT [FK_tblCustomJobT_tblCustomFields] FOREIGN KEY ([tblCustomFieldsID]) REFERENCES [dbo].[tblCustomFields] ([ID])
);

