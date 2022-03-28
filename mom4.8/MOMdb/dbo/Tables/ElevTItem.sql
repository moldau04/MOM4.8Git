CREATE TABLE [dbo].[ElevTItem] (
    [ID]             INT           NOT NULL,
    [ElevT]          INT           NULL,
    [Elev]           INT           NULL,
    [CustomID]       INT           NULL,
    [fDesc]          VARCHAR (50)  NULL,
    [Line]           SMALLINT      NULL,
    [Value]          VARCHAR (500) NULL,
    [Format]         VARCHAR (50)  NULL,
    [fExists]        SMALLINT      NULL,
    [PrimarySyncID]  INT           NULL,
    [LastUpdated]    DATETIME      NULL,
    [LastUpdateUser] VARCHAR (20)  NULL,
    [OrderNo]        INT           NULL,
    [LeadEquip]      INT           NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_ElevTItem_ElevT]
    ON [dbo].[ElevTItem]([ElevT] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_ElevTItem_Elev]
    ON [dbo].[ElevTItem]([Elev] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_ElevTItem_CustomID]
    ON [dbo].[ElevTItem]([CustomID] DESC);

