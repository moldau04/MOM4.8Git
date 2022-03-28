CREATE TABLE [dbo].[tblCommonCustomFields] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [Screen]  VARCHAR(50)          NULL,
    [Label]     VARCHAR (255) NULL,
    [Line]      SMALLINT     NULL,
    [Format]    SMALLINT     NULL,
    [IsDeleted] BIT          CONSTRAINT [DF_tblCommonCustomFields_IsDeleted] DEFAULT ((0)) NULL,
    [OrderNo] INT NULL, 
    [IsAlert] BIT NULL, 
    [TeamMember] VARCHAR(MAX) NULL, 
    [TeamMemberDisplay] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_tblCommonCustomFields] PRIMARY KEY ([ID]) 
);

