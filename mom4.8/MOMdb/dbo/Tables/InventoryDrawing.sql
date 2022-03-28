CREATE TABLE [dbo].[InventoryDrawing] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [InventoryDrawing_InvID] INT             NOT NULL,
    [FileName]               NVARCHAR (100)  NULL,
    [FileServerPath]         VARCHAR (75)    NULL,
    [FileBinary]             VARBINARY (MAX) NULL, 
    CONSTRAINT [PK_InventoryDrawing] PRIMARY KEY ([ID])
);

