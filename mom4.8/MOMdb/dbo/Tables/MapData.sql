CREATE TABLE [dbo].[MapData] (
    [deviceId]  VARCHAR (100)     NOT NULL,
    [latitude]  VARCHAR (50)      NULL,
    [longitude] VARCHAR (50)      NULL,
    [date]      DATETIME          NULL,
    [geoCord]   [sys].[geography] NULL,
    [ID]        INT               IDENTITY (1, 1) NOT NULL, 
    CONSTRAINT [PK_MapData] PRIMARY KEY ([ID])
);

