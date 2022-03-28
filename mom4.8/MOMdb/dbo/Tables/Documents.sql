﻿CREATE TABLE [dbo].[Documents] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Screen]   VARCHAR (20)   NULL,
    [ScreenID] INT            NULL,
    [Line]     SMALLINT       NULL,
    [fDesc]    VARCHAR (255)  NULL,
    [Filename] VARCHAR (1000)   NULL,
    [Path]     VARCHAR (1000)  NULL,
    [Type]     SMALLINT       NULL,
    [Remarks]  VARCHAR (8000) NULL,
    [Custom1]  DATETIME       NULL,
    [Custom2]  DATETIME       NULL,
    [Custom3]  DATETIME       NULL,
    [Custom4]  DATETIME       NULL,
    [Custom5]  DATETIME       NULL,
    [Custom6]  TINYINT        NULL,
    [Custom7]  TINYINT        NULL,
    [Custom8]  TINYINT        NULL,
    [Custom9]  TINYINT        NULL,
    [Custom10] TINYINT        NULL,
    [Custom11] VARCHAR (75)   NULL,
    [Custom12] VARCHAR (75)   NULL,
    [Custom13] VARCHAR (75)   NULL,
    [Custom14] VARCHAR (75)   NULL,
    [Custom15] VARCHAR (75)   NULL,
    [TempID]   VARCHAR (150)  NULL,
    [Date]     DATETIME       CONSTRAINT [DF_Documents_Date] DEFAULT (getdate()) NULL,
    [Subject]  VARCHAR (70)   NULL,
    [Body]     VARCHAR (250)  NULL,
    [Portal]   SMALLINT       NULL,
    [MSVisible] BIT NULL, 
    [lat] VARCHAR(max) NULL,
    [Lng] VARCHAR (max) NULL ,
    [attached_on] DATETIME
    CONSTRAINT [PK__Document__3214EC276FB49575] PRIMARY KEY CLUSTERED ([ID] ASC)
);
