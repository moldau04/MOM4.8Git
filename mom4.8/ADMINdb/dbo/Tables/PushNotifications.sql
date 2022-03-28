CREATE TABLE [dbo].[PushNotifications] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [TokenId]    NVARCHAR (MAX) NOT NULL,
    [DeviceID]   VARCHAR (100)  NOT NULL,
    [DeviceType] VARCHAR (100)  CONSTRAINT [DF_PushNotifications_DeviceType] DEFAULT ('Android') NULL
   
);


