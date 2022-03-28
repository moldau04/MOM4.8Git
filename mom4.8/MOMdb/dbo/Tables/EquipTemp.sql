CREATE TABLE [dbo].[EquipTemp] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [fdesc]         VARCHAR (255)  NULL,
    [Count]         INT            NULL,
    [Remarks]       VARCHAR (8000) NULL,
    [PrimarySyncID] INT            NULL,
    CONSTRAINT [PK_EquipTemp] PRIMARY KEY CLUSTERED ([ID] ASC)
);

