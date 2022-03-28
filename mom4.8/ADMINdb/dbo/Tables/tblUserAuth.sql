CREATE TABLE [dbo].[tblUserAuth] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [DBname]     VARCHAR (50)   NOT NULL,
    [UserID]     INT            NULL,
    [str]        NVARCHAR (400) NOT NULL,
    [used]       INT            NULL,
    [dateupdate] DATETIME       NULL,
    CONSTRAINT [IX_tblUserAuth] UNIQUE NONCLUSTERED ([str] ASC)
);

