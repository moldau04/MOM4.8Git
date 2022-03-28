CREATE TYPE [dbo].[tblTypePOItem] AS TABLE (
    [ID]       INT             NULL,
    [Line]     SMALLINT        NULL,
    [AcctID]   INT             NULL,
    [fDesc]    VARCHAR (8000)  NULL,
    [Quan]     NUMERIC (30, 2) NULL,
    [Price]    NUMERIC (30, 4) NULL,
    [Amount]   NUMERIC (30, 2) NULL,
    [JobID]    INT             NULL,
    [PhaseID]  INT             NULL,
    [Inv]      INT             NULL,
    [Billed]   INT             NULL,
    [Ticket]   INT             NULL,
    [Due]      DATETIME        NULL,
    [TypeID]   INT             NULL,
    [ItemDesc] VARCHAR (255)    NULL,
	[WarehouseID] [varchar](5) NULL,
	[LocationID] [int] NULL,
	[OpSq] [varchar](150) NULL,
	[Selected]     NUMERIC (30, 2) NULL,
	[SelectedQuan]     NUMERIC (30, 2) NULL);





