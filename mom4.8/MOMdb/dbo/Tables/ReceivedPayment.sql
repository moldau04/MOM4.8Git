CREATE TABLE [dbo].[ReceivedPayment] (
    [ID]                  INT             IDENTITY (1, 1) NOT NULL,
    [Loc]                 INT             NULL,
    [Amount]              NUMERIC (30, 2) NULL,
    [PaymentReceivedDate] DATETIME        NULL,
    [PaymentMethod]       SMALLINT        NULL,
    [CheckNumber]         VARCHAR (21)    NULL,
    [AmountDue]           NUMERIC (30, 2) NULL,
    [fDesc]               VARCHAR (250)   NULL,
    [Status]              SMALLINT        NULL,
    [Owner]               INT             NULL, 
	[Batch]               INT             NULL, 
    CONSTRAINT [PK_ReceivedPayment] PRIMARY KEY ([ID])
);

