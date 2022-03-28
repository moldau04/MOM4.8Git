CREATE TABLE [dbo].[tblPingDevice] (
    [deviceID]          VARCHAR (100) NOT NULL,
    [randomID]          VARCHAR (100) NOT NULL,
    [date]              DATETIME      NULL,
    [IsRunning]         SMALLINT      NULL,
    [IsGPSEnabled]      SMALLINT      NULL,
    [BackgroundRefresh] INT           NULL
);
GO
