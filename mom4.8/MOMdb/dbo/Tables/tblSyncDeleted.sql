CREATE TABLE [dbo].[tblSyncDeleted] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [Tbl]       VARCHAR (25)  NULL,
    [Name]      VARCHAR (150) NULL,
    [RefID]     INT           NULL,
    [QBID]      VARCHAR (100) NULL,
    [DateStamp] DATETIME      NULL,
    [Data]      XML           NULL, 
    CONSTRAINT [PK_tblSyncDeleted] PRIMARY KEY ([ID])
);

