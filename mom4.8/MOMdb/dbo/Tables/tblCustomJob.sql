CREATE TABLE [dbo].[tblCustomJob] (
    [JobID]             INT          NULL,
    [tblCustomFieldsID] INT          NULL,
    [Value]             VARCHAR (255) NULL,
    [UpdatedDate]       DATETIME     NULL,
    [Username]          VARCHAR (50) NULL,
	[IsAlert] BIT NULL, 
    [IsTask] BIT NULL, 
    [TeamMember] VARCHAR(MAX) NULL, 
    [TeamMemberDisplay] VARCHAR(MAX) NULL, 
    [UserRole] VARCHAR(MAX) NULL, 
    [UserRoleDisplay] VARCHAR(MAX) NULL, 
    CONSTRAINT [FK_tblCustomJob_tblCustomFields] FOREIGN KEY ([tblCustomFieldsID]) REFERENCES [dbo].[tblCustomFields] ([ID])
);



