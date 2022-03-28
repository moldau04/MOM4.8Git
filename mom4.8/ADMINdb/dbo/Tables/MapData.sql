CREATE TABLE [dbo].[MapData] (
    [deviceId]  VARCHAR (100) NOT NULL,
    [latitude]  VARCHAR (50)  NULL,
    [longitude] VARCHAR (50)  NULL,
    [date]      DATETIME      NULL,
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [SysDate]   DATETIME      CONSTRAINT [DF_MapData_SysDate] DEFAULT (getdate()) NULL,
    [fake]      INT           CONSTRAINT [DF_MapData_Fake] DEFAULT ((0)) NULL,
    [Accuracy]  VARCHAR (50)  NULL,
    [fuser] VARCHAR(50) NULL, 
		[userId] [varchar](50) NULL,
	[battery] VARCHAR(255)NULL,
	[speed] VARCHAR(255)NULL
    CONSTRAINT [PK_MapData] PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO

CREATE NONCLUSTERED INDEX [NC_Date]
    ON [dbo].[MapData]([date] ASC);
GO

CREATE NONCLUSTERED INDEX [NC_Device_Date]
    ON [dbo].[MapData]([deviceId] ASC)
    INCLUDE([date]);
GO

CREATE NONCLUSTERED INDEX [NC_DeviceID]
    ON [dbo].[MapData]([deviceId] ASC);
GO

CREATE NONCLUSTERED INDEX [NC_DeviceIDDate]
    ON [dbo].[MapData]([deviceId] ASC, [date] ASC);
GO
