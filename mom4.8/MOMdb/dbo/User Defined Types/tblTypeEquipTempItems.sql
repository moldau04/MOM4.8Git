CREATE TYPE [dbo].[tblTypeEquipTempItems] AS TABLE (
    [Code]        VARCHAR (25)  NULL,
    [EquipT]      INT           NULL,
    [Elev]        INT           NULL,
    [fDesc]       VARCHAR (255) NULL,
    [Line]        INT           NULL,
    [Lastdate]    DATETIME      NULL,
    [NextDateDue] DATETIME      NULL,
    [Frequency]   INT           NULL,
    [Section]     VARCHAR (50)  NULL,
	[Notes] [text] NULL,
	[LeadEquip] INT NULL
	);

