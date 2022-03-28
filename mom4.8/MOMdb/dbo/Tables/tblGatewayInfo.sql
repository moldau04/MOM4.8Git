CREATE TABLE [dbo].[tblGatewayInfo] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [MerchantId] VARCHAR (100) NULL,
    [LoginId]    VARCHAR (100) NULL,
    [Username]   VARCHAR (20)  NULL,
    [Password]   VARCHAR (200) NULL, 
    CONSTRAINT [PK_tblGatewayInfo] PRIMARY KEY ([ID])
);

