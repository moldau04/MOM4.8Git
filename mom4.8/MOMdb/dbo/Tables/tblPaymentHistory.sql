CREATE TABLE [dbo].[tblPaymentHistory] (
    [TransactionID]  BIGINT           IDENTITY (1000, 1) NOT NULL,
    [InvoiceID]      INT              NULL,
    [TransDate]      DATETIME         NULL,
    [CardNumber]     NVARCHAR (200)   NULL,
    [Cardtype]       VARCHAR (50)     NULL,
    [Amount]         MONEY            NULL,
    [Response]       VARCHAR (MAX)    NULL,
    [RefID]          VARCHAR (500)    NULL,
    [UserID]         VARCHAR (50)     NULL,
    [Screen]         VARCHAR (15)     NULL,
    [Medium]         VARCHAR (15)     NULL,
    [TransType]      VARCHAR (50)     NULL,
    [ResponseCodes]  VARCHAR (MAX)    NULL,
    [Approved]       VARCHAR (50)     NULL,
    [IsSuccess]      SMALLINT         NULL,
    [CustomerID]     INT              NULL,
    [PaymentUID]     UNIQUEIDENTIFIER NULL,
    [GatewayOrderID] VARCHAR (150)    NULL,
    [Routing]        VARCHAR (10)     NULL,
    [BankAccNo]      VARCHAR (25)     NULL,
    [NameAccHolder]  VARCHAR (50)     NULL,
    [FileName]       VARCHAR (500)    NULL,
    [PayType]        CHAR (3)         NULL,
    CONSTRAINT [PK_tblPaymentHistory] PRIMARY KEY CLUSTERED ([TransactionID] ASC)
);



