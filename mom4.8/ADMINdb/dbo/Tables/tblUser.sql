CREATE TABLE [dbo].[tblUser] (
    [Username] NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    [ID]       INT            IDENTITY (1, 1) NOT NULL
);

