CREATE TABLE [dbo].[EquipTItem] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [EquipT]        INT           NOT NULL,
    [Elev]          INT           NOT NULL,
    [fDesc]         VARCHAR (255) NULL,
    [Line]          INT           NULL,
    [Lastdate]      DATETIME      NULL,
    [NextDateDue]   DATETIME      NULL,
    [Frequency]     INT           NULL,
    [Code]          VARCHAR (25)  NULL,
    [section]       VARCHAR (50)  NULL,
    [PrimarySyncID] INT           NULL,
    [Notes]         TEXT          NULL,
    [LeadEquip]     INT           NULL,
    CONSTRAINT [PK_EquipTItem] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_EquipTItem_EquipT]
    ON [dbo].[EquipTItem]([EquipT] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_EquipTItem_Elev]
    ON [dbo].[EquipTItem]([Elev] DESC);

