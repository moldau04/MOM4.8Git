CREATE TABLE [dbo].[tblCustom] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [tblCustomFieldsID] INT          NULL,
    [Line]              SMALLINT     NULL,
    [Value]             VARCHAR (255) NULL,
    CONSTRAINT [PK_tblCustom] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_tblCustom_tblCustomFields] FOREIGN KEY ([tblCustomFieldsID]) REFERENCES [dbo].[tblCustomFields] ([ID])
);

