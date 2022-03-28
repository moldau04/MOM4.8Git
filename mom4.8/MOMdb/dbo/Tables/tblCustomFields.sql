CREATE TABLE [dbo].[tblCustomFields] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [tblTabID]  INT          NULL,
    [Label]     VARCHAR (255) NULL,
    [Line]      SMALLINT     NULL,
    [Format]    SMALLINT     NULL,
    [IsDeleted] BIT          CONSTRAINT [DF_tblCustomFields_IsDeleted] DEFAULT ((0)) NULL,
    [OrderNo] INT NULL, 
    [IsAlert] BIT NULL, 
    [IsTask] BIT NULL, 
    [TeamMember] VARCHAR(MAX) NULL, 
    [TeamMemberDisplay] VARCHAR(MAX) NULL, 
    [UserRole] VARCHAR(MAX) NULL, 
    [UserRoleDisplay] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_tblCustomFields] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_tblCustomFields_tblTabs] FOREIGN KEY ([tblTabID]) REFERENCES [dbo].[tblTabs] ([ID])
);

