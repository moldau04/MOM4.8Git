CREATE TYPE [dbo].[tblTypeTimesheetTicket] AS TABLE (
    [TicketID]    INT             NULL,
    [Reg]         NUMERIC (30, 2) NULL,
    [OT]          NUMERIC (30, 2) NULL,
    [DT]          NUMERIC (30, 2) NULL,
    [TT]          NUMERIC (30, 2) NULL,
    [NT]          NUMERIC (30, 2) NULL,
    [Zone]        NUMERIC (30, 2) NULL,
    [Mileage]     NUMERIC (30, 2) NULL,
    [Misc]        NUMERIC (30, 2) NULL,
    [Toll]        NUMERIC (30, 2) NULL,
    [HourlyRate]  NUMERIC (30, 2) NULL,
    [Extra]       NUMERIC (30, 2) NULL,
    [Empid]       INT             NULL,
    [Custom]      NUMERIC (30, 2) NULL,
    [CustomTick3] TINYINT         NULL,
    [CustomTick2] NUMERIC (30, 2) NULL,
    [CustomTick1] NUMERIC (30, 2) NULL);

