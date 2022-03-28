CREATE TABLE [dbo].[Team] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [JobID]     INT           NULL,
    [Line]      SMALLINT      NULL,
    [Title]     VARCHAR (150) NULL,
    [MomUserID] VARCHAR (50)  NULL,
    [FirstName] VARCHAR (15)  NULL,
    [LastName]  VARCHAR (25)  NULL,
    [Email]     VARCHAR (255) NULL,
    [Mobile]    VARCHAR (28)  NULL
);

