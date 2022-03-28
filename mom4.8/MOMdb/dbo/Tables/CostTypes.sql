CREATE TABLE [dbo].[CostTypes](
			[ID] [int] IDENTITY(1,1) NOT NULL,
			[CostTypes] nvarchar(255),
			[InUse] bit default(0)
			
	
		) ON [PRIMARY]