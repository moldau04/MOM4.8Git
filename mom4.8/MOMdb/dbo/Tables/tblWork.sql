﻿CREATE TABLE [dbo].[tblWork] (
    [ID]         INT             IDENTITY (1, 1) NOT NULL,
    [fDesc]      VARCHAR (50)    NULL,
    [Type]       SMALLINT        NULL,
    [Status]     SMALLINT        NULL,
    [Address]    VARCHAR (8000)  NULL,
    [City]       VARCHAR (50)    NULL,
    [State]      VARCHAR (2)     NULL,
    [Zip]        VARCHAR (10)    NULL,
    [fLong]      INT             NULL,
    [Latt]       INT             NULL,
    [GeoLock]    SMALLINT        NOT NULL,
    [Members]    VARCHAR (100)   NULL,
    [Car]        INT             NULL,
    [Super]      VARCHAR (50)    NULL,
    [DBoard]     SMALLINT        NULL,
    [EN]         INT             NULL,
    [Activity]   CHAR (30)       NULL,
    [Job]        INT             NULL,
    [datDisp]    DATETIME        NULL,
    [JobType]    TINYINT         NULL,
    [fLevel]     TINYINT         NULL,
    [hourlyrate] NUMERIC (30, 2) NULL, 
    [fmseid] VARCHAR(50) NULL,
	[MSHelper]   TINYINT         DEFAULT ('') NULL, 
    CONSTRAINT [PK_tblWork] PRIMARY KEY ([ID])
);

