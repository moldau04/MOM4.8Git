﻿CREATE TABLE [dbo].[tblTimesheetEmp] (
    [ID]           INT             IDENTITY (1, 1) NOT NULL,
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
    [Total]        NUMERIC (30, 2) NULL,
    [Misc]         NUMERIC (30, 2) NULL,
    [Toll]         NUMERIC (30, 2) NULL,
    [FixedHours]   NUMERIC (30, 2) NULL,
    [Salary]       NUMERIC (30, 2) NULL,
    [MileRate]     NUMERIC (30, 2) NULL,
    [HourRate]     NUMERIC (30, 2) NULL,
    [DollarAmount] NUMERIC (30, 2) NULL,
    [Reg1]         NUMERIC (30, 2) NULL,
    [OT1]          NUMERIC (30, 2) NULL,
    [DT1]          NUMERIC (30, 2) NULL,
    [TT1]          NUMERIC (30, 2) NULL,
    [NT1]          NUMERIC (30, 2) NULL,
    [Zone1]        NUMERIC (30, 2) NULL,
    [Mileage1]     NUMERIC (30, 2) NULL,
    [Extra1]       NUMERIC (30, 2) NULL,
    [Misc1]        NUMERIC (30, 2) NULL,
    [Toll1]        NUMERIC (30, 2) NULL,
    [HourRate1]    NUMERIC (30, 2) NULL,
    [Custom]       NUMERIC (30, 2) NULL, 
    CONSTRAINT [PK_tblTimesheetEmp] PRIMARY KEY ([ID])
);

