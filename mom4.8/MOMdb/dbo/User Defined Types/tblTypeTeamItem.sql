CREATE TYPE [dbo].[tblTypeTeamItem] AS TABLE (
    [Line]      SMALLINT      NULL,
	[JobID]     INT           NULL,
    [Title]     VARCHAR (150) NULL,
    [MomUserID] VARCHAR (50)  NULL,
    [FirstName] VARCHAR (15)  NULL,
    [LastName]  VARCHAR (25)  NULL,
    [Email]     VARCHAR (255) NULL,
    [Mobile]    VARCHAR (28)  NULL);

