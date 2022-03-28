CREATE TABLE [dbo].[tblLocationRole] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [Role]     VARCHAR (50) NULL,
    [Username] VARCHAR (50) NULL,
    [Password] VARCHAR (50) NULL,
    [Owner]    INT          NULL, 
    CONSTRAINT [PK_tblLocationRole] PRIMARY KEY ([ID])
);

