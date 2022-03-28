CREATE TABLE [dbo].[tblTestCustomFields] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
	[Line]      SMALLINT     NULL,
	[OrderNo] INT NULL,    
    [Label]     VARCHAR (100) NULL,
    [IsAlert] BIT  NULL,
	[TeamMember] VARCHAR (Max) NULL,
    [Format]    SMALLINT     NULL, 
	[TeamMemberDisplay] [varchar](Max) NULL,
	[UserRoles] [varchar](Max) NULL,
	[UserRolesDisplay] [varchar](Max) NULL
    CONSTRAINT [PK_tblTestCustomFields] PRIMARY KEY CLUSTERED ([ID] ASC)   
);

