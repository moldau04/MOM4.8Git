﻿CREATE TABLE [dbo].[Core_Session_Data](
	[PK] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NULL,
	[User_Token] [nvarchar](500) NULL,
	[Session_Key] [nvarchar](50) NULL,
	[Session_Data] [nvarchar](max) NULL,
 CONSTRAINT [PK_MOM_Session_Data] PRIMARY KEY CLUSTERED 
(
	[PK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
