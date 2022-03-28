CREATE TYPE [dbo].[tblTypeCustomItem] AS TABLE (
    [ID]       INT          NULL,
    [tblTabID] INT          NULL,
    [Label]    VARCHAR (255) NULL,
    [Line]     SMALLINT     NULL,
    [Value]    VARCHAR (255) NULL,
    [Format]   SMALLINT     NULL
	)

