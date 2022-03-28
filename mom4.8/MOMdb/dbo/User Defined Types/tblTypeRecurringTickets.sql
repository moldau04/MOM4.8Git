CREATE TYPE [dbo].[tblTypeRecurringTickets] AS TABLE (
    [Loc]        INT           NULL,
    [Address]    VARCHAR (255) NULL,
    [city]       VARCHAR (50)  NULL,
    [state]      VARCHAR (2)   NULL,
    [zip]        VARCHAR (10)  NULL,
    [calldate]   DATETIME      NULL,
    [scheduledt] DATETIME      NULL,
    [assigned]   INT           NULL,
    [worker]     VARCHAR (50)  NULL,
    [category]   VARCHAR (50)  NULL,
    [Elev]       INT           NULL,
    [Owner]      INT           NULL,
    [Jobremarks] VARCHAR (MAX) NULL,
    [remarks]    VARCHAR (255) NULL,
    [Job]        INT           NULL);

