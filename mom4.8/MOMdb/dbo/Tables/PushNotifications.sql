CREATE TABLE [dbo].[PushNotifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TokenId] [nvarchar](max) NOT NULL,
	[DeviceID] [varchar](100) NOT NULL,
	[DeviceType] [varchar](50) NULL,
	[FUser] [varchar](50) NULL,
	[UserID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PushNotifications] ADD  DEFAULT ('Android') FOR [DeviceType]
GO