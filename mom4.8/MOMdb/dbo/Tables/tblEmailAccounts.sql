CREATE TABLE [dbo].[tblEmailAccounts] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [InServer]     VARCHAR (100) NULL,
    [InServerType] VARCHAR (10)  NULL,
    [InUsername]   VARCHAR (100) NULL,
    [InPassword]   VARCHAR (50)  NULL,
    [InPort]       INT           NULL,
    [OutServer]    VARCHAR (100) NULL,
    [OutUsername]  VARCHAR (100) NULL,
    [OutPassword]  VARCHAR (50)  NULL,
    [OutPort]      INT           NULL,
    [SSL]          BIT           NULL,
    [UserId]       INT           NULL,
    [LastFetch]    DATETIME      NULL,
	[BccEmail]	   VARCHAR(100)  NULL, 
    [TakeASentEmailCopy] BIT NULL, 
    CONSTRAINT [PK_tblEmailAccounts] PRIMARY KEY ([ID])
);

