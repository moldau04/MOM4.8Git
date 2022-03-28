		CREATE TABLE [dbo].[tblSchedule](
			[ID] [int] IDENTITY(1,1) NOT NULL,
			[Label] [nvarchar](50) NULL,
			[Description] [nvarchar](50) NULL,
			[Type] [nvarchar](50) NULL,
		 CONSTRAINT [PK_tblSchedule] PRIMARY KEY CLUSTERED 
		(
			[ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
