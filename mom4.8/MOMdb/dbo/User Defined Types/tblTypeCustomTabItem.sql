CREATE TYPE [dbo].[tblTypeCustomTabItem] AS TABLE (
    [ID]          INT          NULL,
    [tblTabID]    INT          NULL,
    [Label]       VARCHAR (255) NULL,
    [Line]        SMALLINT     NULL,
    [Value]       VARCHAR (255) NULL,
    [Format]      SMALLINT     NULL,
    [UpdatedDate] DATETIME     NULL,
    [Username]    VARCHAR (50) NULL,
	[IsAlert] BIT NULL, 
    [IsTask] BIT NULL, 
    [TeamMember] VARCHAR(MAX) NULL, 
    [TeamMemberDisplay] VARCHAR(MAX) NULL,
    [UserRole] VARCHAR(MAX) NULL, 
    [UserRoleDisplay] VARCHAR(MAX) NULL
);