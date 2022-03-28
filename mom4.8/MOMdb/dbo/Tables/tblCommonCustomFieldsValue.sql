CREATE TABLE [dbo].[tblCommonCustomFieldsValue] (
	[Screen]            VARCHAR(50) NOT NULL,
    [Ref]               INT          NOT NULL,
    [tblCommonCustomFieldsID] INT          NOT NULL,
    [Value]             VARCHAR (255) NULL,
    [UpdatedDate]       DATETIME     NULL,
    [Username]          VARCHAR (50) NULL,
	[IsAlert] BIT NULL, 
    [TeamMember] VARCHAR(MAX) NULL, 
    [TeamMemberDisplay] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_tblCommonCustomFieldsValue] PRIMARY KEY ([Ref], [tblCommonCustomFieldsID], [Screen]) 
);



