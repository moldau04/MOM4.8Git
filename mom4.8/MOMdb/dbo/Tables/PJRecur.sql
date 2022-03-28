CREATE TABLE [dbo].[PJRecur](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[Ref] [varchar](50) NULL,
	[fDesc] [varchar](8000) NULL,
	[Amount] [numeric](30, 2) NULL,
	[Vendor] [int] NULL,
	[Status] [smallint] NULL,
	[Terms] [smallint] NULL,
	[PO] [int] NULL,
	[TRID] [int] NULL,
	[Spec] [smallint] NULL,
	[IDate] [datetime] NOT NULL,
	[UseTax] [numeric](30, 2) NULL,
	[Disc] [numeric](30, 4) NULL,
	[Custom1] [varchar](50) NULL,
	[Custom2] [varchar](50) NULL,
	[ReqBy] [int] NULL,
	[VoidR] [varchar](75) NULL,
	[ReceivePO] [int] NULL,
	[IfPaid] [int] NULL,
	[Frequency] [int] NULL
);

