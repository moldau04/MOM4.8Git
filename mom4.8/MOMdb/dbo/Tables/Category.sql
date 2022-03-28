CREATE TABLE [dbo].[Category] (
    [Type]       VARCHAR (30)   NULL,
    [Count]      SMALLINT       NULL,
    [Remarks]    VARCHAR (8000) NULL,
    [Color]      SMALLINT       NULL,
    [Icon]       IMAGE          NULL,
    [chargeable] BIT            NULL,
    [ISDefault]  SMALLINT       NULL,
	[Status]  BIT       NULL
);

