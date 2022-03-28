CREATE TABLE [dbo].[tblWorkflowFields] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
	[Line]      SMALLINT     NULL,
	[OrderNo] INT NULL,    
    [Label]     VARCHAR (100) NULL,
    [IsAlert] BIT  NULL,
	[TeamMember] VARCHAR (Max) NULL,
    [Format]    SMALLINT     NULL, 
	[TeamMemberDisplay] [varchar](Max) NULL	
    CONSTRAINT [PK_tblWorkflowFields] PRIMARY KEY CLUSTERED ([ID] ASC)   
);