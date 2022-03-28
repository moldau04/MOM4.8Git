CREATE TABLE [dbo].[tblProjectStage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL, 
	[Label] [varchar](50) NULL,
    [ChartColors] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_ProjectStage] PRIMARY KEY ([ID])
)