CREATE TABLE [dbo].[tblTestCustom] (
    [ID]                INT          IDENTITY (1, 1) NOT NULL,
    [tblTestCustomFieldsID] INT          NULL,
    [Line]              SMALLINT     NULL,
    [Value]             VARCHAR (50) NULL,
    CONSTRAINT [PK_tblTestCustom] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_tblTestCustom_tblTestCustomFields] FOREIGN KEY ([tblTestCustomFieldsID]) REFERENCES [dbo].[tblTestCustomFields] ([ID]) ON DELETE cascade
);

