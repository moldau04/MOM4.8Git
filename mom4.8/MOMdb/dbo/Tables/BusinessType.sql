CREATE TABLE [dbo].[BusinessType] (
    [ID]        int  Identity(1,1)  NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Count]       INT            NOT NULL,
    [Label] VARCHAR(50) NULL, 
    CONSTRAINT [PK_BT] PRIMARY KEY CLUSTERED ([ID] ASC)
);

