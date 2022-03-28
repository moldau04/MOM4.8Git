CREATE TABLE [dbo].[RepDetail] (
    [id]              INT          IDENTITY (1, 1) NOT NULL,
    [EquipTItem]      INT          NOT NULL,
    [fwork]           INT          NOT NULL,
    [Elev]            INT          NULL,
    [ticketID]        INT          NULL,
    [Code]            VARCHAR (25) NULL,
    [Lastdate]        DATETIME     NULL,
    [NextDateDue]     DATETIME     NULL,
    [OrigLastdate]    DATETIME     NULL,
    [OrigNextDateDue] DATETIME     NULL,
    [comment]         TEXT         NULL,
    [status]          VARCHAR (50) NULL,
	[fDesc]           TEXT         DEFAULT ('') NULL, 
    CONSTRAINT [PK_RepDetail] PRIMARY KEY ([id])
);

