﻿CREATE TYPE [dbo].[tblTypeTimesheetEmpl] AS TABLE (
    [TimesheetID]  INT             NULL,
    [EmpID]        INT             NULL,
    [Pay]          SMALLINT        NULL,
    [PayMethod]    SMALLINT        NULL,
    [Reg]          NUMERIC (30, 2) NULL,
    [OT]           NUMERIC (30, 2) NULL,
    [DT]           NUMERIC (30, 2) NULL,
    [TT]           NUMERIC (30, 2) NULL,
    [NT]           NUMERIC (30, 2) NULL,
    [Holiday]      NUMERIC (30, 2) NULL,
    [Vacation]     NUMERIC (30, 2) NULL,
    [SickTime]     NUMERIC (30, 2) NULL,
    [Zone]         NUMERIC (30, 2) NULL,
    [Reimb]        NUMERIC (30, 2) NULL,
    [Mileage]      NUMERIC (30, 2) NULL,
    [Bonus]        NUMERIC (30, 2) NULL,
    [Extra]        NUMERIC (30, 2) NULL,
    [Misc]         NUMERIC (30, 2) NULL,
    [Toll]         NUMERIC (30, 2) NULL,
    [Total]        NUMERIC (30, 2) NULL,
    [FixedHours]   NUMERIC (30, 2) NULL,
    [Salary]       NUMERIC (30, 2) NULL,
    [MileRate]     NUMERIC (30, 2) NULL,
    [HourRate]     NUMERIC (30, 2) NULL,
    [DollarAmount] NUMERIC (30, 2) NULL,
    [Custom]       NUMERIC (30, 2) NULL);

