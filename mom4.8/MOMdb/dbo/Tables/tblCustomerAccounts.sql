CREATE TABLE [dbo].[tblCustomerAccounts] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [OwnerID]   INT          NULL,
    [RoutingNo] VARCHAR (10) NULL,
    [AccountNo] VARCHAR (20) NULL,
    [Name]      VARCHAR (20) NULL, 
    CONSTRAINT [PK_tblCustomerAccounts] PRIMARY KEY ([ID])
);

