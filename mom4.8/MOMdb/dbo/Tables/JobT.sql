CREATE TABLE [dbo].[JobT] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [fDesc]        VARCHAR (50)   NULL,
    [Type]         SMALLINT       NULL,
    [NRev]         SMALLINT       NULL,
    [NDed]         SMALLINT       NULL,
    [Count]        INT            NULL,
    [Remarks]      VARCHAR (8000) NULL,
    [InvExp]       INT            NULL,
    [InvServ]      INT            NULL,
    [Wage]         INT            NULL,
	[UnrecognizedRevenue] int	  NULL,
	[UnrecognizedExpense] int	  NULL,
	[RetainageReceivable] int	  NULL,
    [CType]        VARCHAR (15)   NULL,
    [Status]       TINYINT        NULL,
    [Charge]       TINYINT        NULL,
    [Post]         TINYINT        NULL,
    [fInt]         TINYINT        NULL,
    [GLInt]        INT            NULL,
    [JobClose]     TINYINT        NULL,
    [TemplateRev]  VARCHAR (150)  NULL,
    [RevRemarks]   VARCHAR (MAX)  NULL,
    [AlertType]    SMALLINT       NULL,
    [AlertMgr]     BIT            NULL,
    [MilestoneMgr] BIT            NULL, 
    [fmseid] INT NULL, 
    [TargetHPermission] NUMERIC(30, 2) NULL, 
    [OHPer] NUMERIC(30, 4) NULL, 
	[COMMSPer] NUMERIC(30, 4) NULL,
	[MARKUPPer] NUMERIC(30, 4) NULL,
	[STaxName] VARCHAR(25) NULL
    CONSTRAINT [PK_JobT] PRIMARY KEY ([ID]), 
    [EstimateType] VARCHAR(50) NULL, 
    [IsSglBilAmt] BIT NULL
);



