
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/14/2020 15:50:52
-- Generated from EDMX file: F:\Turlock\Workspaces\MOM2\MOM-NewDesign-Staging\BryntumGantt\Entities.edmx
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Assignments_Resources]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Assignments] DROP CONSTRAINT [FK_Assignments_Resources];
GO
IF OBJECT_ID(N'[dbo].[FK_Assignments_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Assignments] DROP CONSTRAINT [FK_Assignments_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_Dependencies_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Dependencies] DROP CONSTRAINT [FK_Dependencies_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_Dependencies_Tasks1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Dependencies] DROP CONSTRAINT [FK_Dependencies_Tasks1];
GO
IF OBJECT_ID(N'[dbo].[FK_Tasks_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_Tasks_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_CalendarDays_Calendars1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CalendarDays] DROP CONSTRAINT [FK_CalendarDays_Calendars1];
GO
IF OBJECT_ID(N'[dbo].[FK_Calendars_Calendars1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calendars] DROP CONSTRAINT [FK_Calendars_Calendars1];
GO
IF OBJECT_ID(N'[dbo].[FK_Resources_Calendars1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_Resources_Calendars1];
GO
IF OBJECT_ID(N'[dbo].[FK_Tasks_Calendars1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_Tasks_Calendars1];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskSegments_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskSegments] DROP CONSTRAINT [FK_TaskSegments_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_CalendarDays_Calendars]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CalendarDays] DROP CONSTRAINT [FK_CalendarDays_Calendars];
GO
IF OBJECT_ID(N'[dbo].[FK_Calendars_Calendars]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Calendars] DROP CONSTRAINT [FK_Calendars_Calendars];
GO
IF OBJECT_ID(N'[dbo].[FK_Resources_Calendars]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_Resources_Calendars];
GO
IF OBJECT_ID(N'[dbo].[FK_Tasks_Calendars]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_Tasks_Calendars];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Assignments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Assignments];
GO
IF OBJECT_ID(N'[dbo].[CalendarDays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalendarDays];
GO
IF OBJECT_ID(N'[dbo].[Calendars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Calendars];
GO
IF OBJECT_ID(N'[dbo].[Dependencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dependencies];
GO
IF OBJECT_ID(N'[dbo].[Options]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Options];
GO
IF OBJECT_ID(N'[dbo].[Resources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resources];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[TaskSegments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskSegments];
GO





-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Assignments'
CREATE TABLE [dbo].[Assignments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TaskIdRaw] int  NOT NULL,
    [ResourceIdRaw] int  NOT NULL,
    [Units] int  NOT NULL
);
GO

-- Creating table 'CalendarDays'
CREATE TABLE [dbo].[CalendarDays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CalendarIdRaw] int  NOT NULL,
    [Name] nvarchar(255)  NULL,
    [Type] nvarchar(45)  NOT NULL,
    [Date] datetime  NULL,
    [AvailabilityRaw] nvarchar(255)  NULL,
    [Weekday] int  NULL,
    [OverrideStartDate] datetime  NULL,
    [OverrideEndDate] datetime  NULL,
    [IsWorkingDayRaw] tinyint  NULL,
    [Cls] nvarchar(45)  NULL
);
GO

-- Creating table 'Calendars'
CREATE TABLE [dbo].[Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [parentIdRaw] int  NULL,
    [Name] nvarchar(255)  NULL,
    [DaysPerMonth] int  NULL,
    [DaysPerWeek] int  NULL,
    [HoursPerDay] int  NULL,
    [WeekendsAreWorkdaysRaw] tinyint  NULL,
    [WeekendFirstDay] int  NULL,
    [WeekendSecondDay] int  NULL,
    [DefaultAvailabilityRaw] nvarchar(255)  NULL
);
GO

-- Creating table 'Dependencies'
CREATE TABLE [dbo].[Dependencies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FromIdRaw] int  NOT NULL,
    [ToIdRaw] int  NOT NULL,
    [Type] int  NULL,
    [Cls] nvarchar(255)  NULL,
    [Lag] decimal(18,2)  NULL,
    [LagUnit] nvarchar(10)  NULL
);
GO

-- Creating table 'Options'
CREATE TABLE [dbo].[Options] (
    [name] varchar(45)  NOT NULL,
    [value] varchar(45)  NULL,
    [dt] binary(8)  NOT NULL
);
GO

-- Creating table 'Resources'
CREATE TABLE [dbo].[Resources] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [CalendarIdRaw] int  NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [parentIdRaw] int  NULL,
    [Name] nvarchar(255)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Duration] decimal(18,2)  NULL,
    [DurationUnit] nvarchar(255)  NULL,
    [PercentDone] decimal(18,2)  NULL,
    [SchedulingMode] nvarchar(255)  NULL,
    [BaselineStartDate] datetime  NULL,
    [BaselineEndDate] datetime  NULL,
    [BaselinePercentDone] decimal(18,2)  NULL,
    [Cls] nvarchar(255)  NULL,
    [CalendarIdRaw] int  NULL,
    [index] int  NULL,
    [expanded] bit  NOT NULL,
    [Effort] decimal(18,2)  NULL,
    [EffortUnit] varchar(255)  NULL,
    [Note] varchar(255)  NULL,
    [ConstraintType] varchar(255)  NULL,
    [ConstraintDate] datetime  NULL,
    [ManuallyScheduled] bit  NOT NULL,
    [Draggable] bit  NOT NULL,
    [Resizable] bit  NOT NULL,
    [Rollup] bit  NOT NULL,
    [ShowInTimeline] bit  NOT NULL,
    [Color] nvarchar(255)  NULL,
    [PlannerID] int  NULL,
    [ProjectID] int  NULL,
    [TaskType] nvarchar(max)  NULL
);
GO

-- Creating table 'TaskSegments'
CREATE TABLE [dbo].[TaskSegments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TaskIdRaw] int  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NULL,
    [Duration] decimal(18,2)  NULL,
    [DurationUnit] nvarchar(255)  NULL,
    [Cls] nvarchar(255)  NULL
);
GO
SET IDENTITY_INSERT [dbo].[Assignments] ON 

INSERT [dbo].[Assignments] ([Id], [TaskIdRaw], [ResourceIdRaw], [Units]) VALUES
(1, 4, 1, 50)
,(2, 4, 2, 50)

SET IDENTITY_INSERT [dbo].[Assignments] OFF

SET IDENTITY_INSERT [dbo].[CalendarDays] ON 

INSERT [dbo].[CalendarDays] ([Id], [CalendarIdRaw], [Name], [Type], [Date], [AvailabilityRaw], [Weekday], [OverrideStartDate], [OverrideEndDate], [IsWorkingDayRaw], [Cls]) VALUES
(1, 1, N'Some big holiday', N'DAY', CAST(0x59320B00 AS Date), NULL, NULL, NULL, NULL, 0, N'gnt-national-holiday')
,(2, 2, N'Mats''s birthday', N'DAY', CAST(0x58320B00 AS Date), NULL, NULL, NULL, NULL, 0, N'gnt-national-holiday')
,(3, 2, N'Bryntum company holiday', N'DAY', CAST(0x6B320B00 AS Date), NULL, NULL, NULL, NULL, 0, N'gnt-company-holiday')
,(4, 2, N'Bryntum 1st birthday', N'DAY', CAST(0x9A330B00 AS Date), NULL, NULL, NULL, NULL, 0, NULL)
,(5, 2, N'Half working day', N'DAY', CAST(0x7C350B00 AS Date), N'08:00-12:00', NULL, NULL, NULL, 1, NULL)
,(6, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, NULL, -1, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 0, NULL)
,(7, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, NULL, 0, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 0, NULL)
,(8, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, N'08:00-12:00', 1, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 1, NULL)
,(9, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, N'13:00-15:00', 2, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 1, NULL)
,(10, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, NULL, 3, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 0, NULL)
,(11, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, N'08:00-12:00', 4, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 1, NULL)
,(12, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, NULL, 5, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 0, NULL)
,(13, 2, N'Non standard week', N'WEEKDAYOVERRIDE', NULL, NULL, 6, CAST(0x7A350B00 AS Date), CAST(0x80350B00 AS Date), 0, NULL)
,(14, 2, N'Non standard feb week', N'WEEKDAYOVERRIDE', NULL, NULL, -1, CAST(0x5D350B00 AS Date), CAST(0x60350B00 AS Date), 0, NULL)
,(15, 2, N'Non standard feb week', N'WEEKDAYOVERRIDE', NULL, NULL, 0, CAST(0x5D350B00 AS Date), CAST(0x60350B00 AS Date), 0, NULL)
,(16, 2, N'Non standard feb week', N'WEEKDAYOVERRIDE', NULL, N'08:00-12:00', 1, CAST(0x5D350B00 AS Date), CAST(0x60350B00 AS Date), 1, NULL)
,(17, 2, N'Non standard feb week', N'WEEKDAYOVERRIDE', NULL, N'13:00-15:00', 2, CAST(0x5D350B00 AS Date), CAST(0x60350B00 AS Date), 1, NULL)

SET IDENTITY_INSERT [dbo].[CalendarDays] OFF

SET IDENTITY_INSERT [dbo].[Calendars] ON 

INSERT [dbo].[Calendars] ([Id], [parentIdRaw], [Name], [DaysPerMonth], [DaysPerWeek], [HoursPerDay], [WeekendsAreWorkdaysRaw], [WeekendFirstDay], [WeekendSecondDay], [DefaultAvailabilityRaw]) VALUES
(1, NULL, N'General', 20, 5, 8, 0, 6, 0, N'08:00-12:00|13:00-17:00')
,(2, 1, N'Holidays', 20, 5, 8, 0, 6, 0, N'08:00-12:00')
,(3, NULL, N'Night shift', 20, 5, 8, 0, 6, 0, N'00:00-06:00|22:00-24:00')

SET IDENTITY_INSERT [dbo].[Calendars] OFF

SET IDENTITY_INSERT [dbo].[Dependencies] ON 

INSERT [dbo].[Dependencies] ([Id], [FromIdRaw], [ToIdRaw], [Type], [Cls], [Lag], [LagUnit]) VALUES
(1, 8, 9, 2, N'', 0, N'd')
,(2, 13, 14, 2, N'', 0, N'd')
,(3, 14, 15, 2, N'', 0, N'd')
,(4, 16, 17, 0, N'', 0, N'd')
,(5, 15, 16, 0, N'', 0, N'd')
,(6, 17, 18, 2, N'', 0, N'd')
,(7, 7, 3, 2, N'', 0, N'd')
,(8, 7, 18, 2, N'', 0, N'd')
,(9, 10, 11, 2, N'', 0, N'd')
,(10, 11, 12, 0, N'', 0, N'd')

SET IDENTITY_INSERT [dbo].[Dependencies] OFF

INSERT [dbo].[Options] ([name], [value],[dt]) VALUES 
(N'projectCalendar', N'1',0)
,(N'revision', N'1',0)

SET IDENTITY_INSERT [dbo].[Resources] ON 

INSERT [dbo].[Resources] ([Id], [Name], [CalendarIdRaw]) VALUES
(1, N'Mats', NULL)
,(2, N'Nickolay', NULL)
,(3, N'Goran', NULL)
,(4, N'Dan', NULL)
,(5, N'Jake', NULL)
,(6, N'Kim', NULL)

SET IDENTITY_INSERT [dbo].[Resources] OFF

SET IDENTITY_INSERT [dbo].[Tasks] ON 

INSERT [dbo].[Tasks] ([Id], [parentIdRaw], [Name], [StartDate], [EndDate], [Duration], [DurationUnit], [PercentDone], [SchedulingMode], [BaselineStartDate], [BaselineEndDate], [BaselinePercentDone], [Cls], [index], [CalendarIdRaw], [expanded], [Effort], [EffortUnit], [Note], [ConstraintType], [ConstraintDate], [ManuallyScheduled], [Draggable], [Resizable], [Rollup], [ShowInTimeline], ProjectID) VALUES
(1, NULL, N'Main project', CAST(0x0000A0C10083D600 AS DateTime), CAST(0x0000A132011826C0 AS DateTime), 82, N'd', 11, NULL, NULL, NULL, NULL, N'', 0, NULL, 1, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(2, NULL, N'Second project', CAST(0x0000A0CF0083D600 AS DateTime), CAST(0x0000A0FA011826C0 AS DateTime), 32, N'd', 0, NULL, NULL, NULL, NULL, N'', 1, NULL, 1, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(3, NULL, N'Release', CAST(0x0000A132011826C0 AS DateTime), CAST(0x0000A132011826C0 AS DateTime), 0, N'd', 0, NULL, NULL, NULL, NULL, N'', 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 1, 0)
,(4, 1, N'Initial phase', CAST(0x0000A0C10083D600 AS DateTime), CAST(0x0000A0E1011826C0 AS DateTime), 25, N'd', 70, NULL, NULL, NULL, NULL, N'', 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(5, 1, N'Alpha', CAST(0x0000A0E40083D600 AS DateTime), CAST(0x0000A101011826C0 AS DateTime), 22, N'd', 0, NULL, NULL, NULL, NULL, N'', 1, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 1, 0)
,(6, 1, N'Beta', CAST(0x0000A0F90083D600 AS DateTime), CAST(0x0000A132011826C0 AS DateTime), 42, N'd', 0, NULL, NULL, NULL, NULL, N'', 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 1, 0)
,(7, 1, N'Marketing', CAST(0x0000A1070083D600 AS DateTime), CAST(0x0000A132011826C0 AS DateTime), 32, N'd', 0, NULL, NULL, NULL, NULL, N'', 3, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 1, 0)
,(8, 2, N'Research', CAST(0x0000A0CF0083D600 AS DateTime), CAST(0x0000A0FA011826C0 AS DateTime), 32, N'd', 60, NULL, NULL, NULL, NULL, N'', 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(9, 2, N'Test implementation', CAST(0x0000A0FA011826C0 AS DateTime), CAST(0x0000A0FA011826C0 AS DateTime), 0, N'd', 0, NULL, NULL, NULL, NULL, N'', 1, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(10, 5, N'Research', CAST(0x0000A0E40083D600 AS DateTime), CAST(0x0000A0EF011826C0 AS DateTime), 10, N'd', 0, NULL, NULL, NULL, NULL, N'', 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(11, 5, N'First implementation', CAST(0x0000A0F20083D600 AS DateTime), CAST(0x0000A0F6011826C0 AS DateTime), 5, N'd', 0, NULL, NULL, NULL, NULL, N'', 1, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(12, 5, N'Tests', CAST(0x0000A0F50083D600 AS DateTime), CAST(0x0000A101011826C0 AS DateTime), 9, N'd', 0, NULL, NULL, NULL, NULL, N'', 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(13, 6, N'Refactoring after Alpha', CAST(0x0000A0F90083D600 AS DateTime), CAST(0x0000A104011826C0 AS DateTime), 10, N'd', 0, NULL, NULL, NULL, NULL, N'', 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(14, 6, N'Tests', CAST(0x0000A1070083D600 AS DateTime), CAST(0x0000A10B011826C0 AS DateTime), 5, N'd', 0, NULL, NULL, NULL, NULL, N'', 1, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(15, 6, N'Internal beta', CAST(0x0000A10E0083D600 AS DateTime), CAST(0x0000A120011826C0 AS DateTime), 15, N'd', 0, NULL, NULL, NULL, NULL, N'', 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(16, 6, N'Additional testing', CAST(0x0000A10E0083D600 AS DateTime), CAST(0x0000A12A011826C0 AS DateTime), 21, N'd', 0, NULL, NULL, NULL, NULL, N'', 3, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(17, 6, N'Public beta', CAST(0x0000A10E0083D600 AS DateTime), CAST(0x0000A131011826C0 AS DateTime), 26, N'd', 0, NULL, NULL, NULL, NULL, N'', 4, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
,(18, 6, N'Release', CAST(0x0000A132011826C0 AS DateTime), CAST(0x0000A132011826C0 AS DateTime), 0, N'd', 0, NULL, NULL, NULL, NULL, N'', 5, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, 1, 1, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[Tasks] OFF
SET IDENTITY_INSERT [dbo].[TaskSegments] ON 

INSERT [dbo].[TaskSegments] ([Id], [TaskIdRaw], [StartDate], [EndDate], [Duration], [DurationUnit], [Cls]) VALUES
(1, 4, CAST(0x0000A0C10083D600 AS DateTime), CAST(0x0000A0C2011826C0 AS DateTime), 2, N'd', NULL)
,(2, 4, CAST(0x0000A0C40083D600 AS DateTime), CAST(0x0000A0C8011826C0 AS DateTime), 3, N'd', NULL)
,(3, 4, CAST(0x0000A0CA0083D600 AS DateTime), CAST(0x0000A0E5011826C0 AS DateTime), 20, N'd', NULL)
SET IDENTITY_INSERT [dbo].[TaskSegments] OFF

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Assignments'
ALTER TABLE [dbo].[Assignments]
ADD CONSTRAINT [PK_Assignments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CalendarDays'
ALTER TABLE [dbo].[CalendarDays]
ADD CONSTRAINT [PK_CalendarDays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [PK_Calendars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Dependencies'
ALTER TABLE [dbo].[Dependencies]
ADD CONSTRAINT [PK_Dependencies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [name] in table 'Options'
ALTER TABLE [dbo].[Options]
ADD CONSTRAINT [PK_Options]
    PRIMARY KEY CLUSTERED ([name] ASC);
GO

-- Creating primary key on [Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [PK_Resources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskSegments'
ALTER TABLE [dbo].[TaskSegments]
ADD CONSTRAINT [PK_TaskSegments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ResourceIdRaw] in table 'Assignments'
ALTER TABLE [dbo].[Assignments]
ADD CONSTRAINT [FK_Assignments_Resources]
    FOREIGN KEY ([ResourceIdRaw])
    REFERENCES [dbo].[Resources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Assignments_Resources'
CREATE INDEX [IX_FK_Assignments_Resources]
ON [dbo].[Assignments]
    ([ResourceIdRaw]);
GO

-- Creating foreign key on [TaskIdRaw] in table 'Assignments'
ALTER TABLE [dbo].[Assignments]
ADD CONSTRAINT [FK_Assignments_Tasks]
    FOREIGN KEY ([TaskIdRaw])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Assignments_Tasks'
CREATE INDEX [IX_FK_Assignments_Tasks]
ON [dbo].[Assignments]
    ([TaskIdRaw]);
GO

-- Creating foreign key on [FromIdRaw] in table 'Dependencies'
ALTER TABLE [dbo].[Dependencies]
ADD CONSTRAINT [FK_Dependencies_Tasks]
    FOREIGN KEY ([FromIdRaw])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Dependencies_Tasks'
CREATE INDEX [IX_FK_Dependencies_Tasks]
ON [dbo].[Dependencies]
    ([FromIdRaw]);
GO

-- Creating foreign key on [ToIdRaw] in table 'Dependencies'
ALTER TABLE [dbo].[Dependencies]
ADD CONSTRAINT [FK_Dependencies_Tasks1]
    FOREIGN KEY ([ToIdRaw])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Dependencies_Tasks1'
CREATE INDEX [IX_FK_Dependencies_Tasks1]
ON [dbo].[Dependencies]
    ([ToIdRaw]);
GO

-- Creating foreign key on [parentIdRaw] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Tasks_Tasks]
    FOREIGN KEY ([parentIdRaw])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tasks_Tasks'
CREATE INDEX [IX_FK_Tasks_Tasks]
ON [dbo].[Tasks]
    ([parentIdRaw]);
GO

-- Creating foreign key on [CalendarIdRaw] in table 'CalendarDays'
ALTER TABLE [dbo].[CalendarDays]
ADD CONSTRAINT [FK_CalendarDays_Calendars1]
    FOREIGN KEY ([CalendarIdRaw])
    REFERENCES [dbo].[Calendars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CalendarDays_Calendars1'
CREATE INDEX [IX_FK_CalendarDays_Calendars1]
ON [dbo].[CalendarDays]
    ([CalendarIdRaw]);
GO

-- Creating foreign key on [parentIdRaw] in table 'Calendars'
ALTER TABLE [dbo].[Calendars]
ADD CONSTRAINT [FK_Calendars_Calendars1]
    FOREIGN KEY ([parentIdRaw])
    REFERENCES [dbo].[Calendars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Calendars_Calendars1'
CREATE INDEX [IX_FK_Calendars_Calendars1]
ON [dbo].[Calendars]
    ([parentIdRaw]);
GO

-- Creating foreign key on [CalendarIdRaw] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_Resources_Calendars1]
    FOREIGN KEY ([CalendarIdRaw])
    REFERENCES [dbo].[Calendars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Resources_Calendars1'
CREATE INDEX [IX_FK_Resources_Calendars1]
ON [dbo].[Resources]
    ([CalendarIdRaw]);
GO

-- Creating foreign key on [CalendarIdRaw] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_Tasks_Calendars1]
    FOREIGN KEY ([CalendarIdRaw])
    REFERENCES [dbo].[Calendars]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Tasks_Calendars1'
CREATE INDEX [IX_FK_Tasks_Calendars1]
ON [dbo].[Tasks]
    ([CalendarIdRaw]);
GO

-- Creating foreign key on [TaskIdRaw] in table 'TaskSegments'
ALTER TABLE [dbo].[TaskSegments]
ADD CONSTRAINT [FK_TaskSegments_Tasks]
    FOREIGN KEY ([TaskIdRaw])
    REFERENCES [dbo].[Tasks]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskSegments_Tasks'
CREATE INDEX [IX_FK_TaskSegments_Tasks]
ON [dbo].[TaskSegments]
    ([TaskIdRaw]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------