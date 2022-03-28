CREATE TABLE [dbo].[Warehouse] (
    [ID]       VARCHAR (5)    NOT NULL,
    [Name]     VARCHAR (25)   NULL,
    [Type]     INT            NULL,
    [Location] INT            NULL,
    [Remarks]  VARCHAR (8000) NULL,
    [Count]    INT            NULL,
	[Multi] [bit] NULL,
	[EN] [int] NULL, 
    [status] INT NULL
);

