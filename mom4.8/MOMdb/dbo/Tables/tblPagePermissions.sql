CREATE TABLE [dbo].[tblPagePermissions] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [User]   INT NULL,
    [Page]   INT NULL,
    [Access] BIT NULL,
    [View]   BIT NULL,
    [Edit]   BIT NULL,
    [Add]    BIT NULL,
    [Delete] BIT NULL, 
    CONSTRAINT [PK_tblPagePermissions] PRIMARY KEY ([ID])
);

