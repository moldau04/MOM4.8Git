CREATE TABLE [dbo].[Alarms] (
    [ID]      INT       NOT NULL,
    [Type]    CHAR (10) NULL,
    [Record]  CHAR (15) NULL,
    [fDate]   DATETIME  NULL,
    [fTime]   CHAR (5)  NULL,
    [Message] TEXT      NULL
);

