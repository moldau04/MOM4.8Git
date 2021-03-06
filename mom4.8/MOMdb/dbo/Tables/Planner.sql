CREATE TABLE [dbo].[Planner](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PID] [int] NULL,
	[Desc] [varchar](500) NULL,
	[Type] VARCHAR(50) NULL, 
	CreatedDt Datetime NULL,
	UpdatedDt Datetime NULL,
	CreatedBy Varchar(50) NULL,
	UpdatedBy Varchar(50) NULL,
    CONSTRAINT [PK_Planner] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]