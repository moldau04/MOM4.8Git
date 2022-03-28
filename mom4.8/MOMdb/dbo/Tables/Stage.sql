CREATE TABLE [dbo].[Stage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Count] [int] NOT NULL,
	[Label] [varchar](50) NULL,
 [Type] NVARCHAR(50) NULL, 
    [Probability] NVARCHAR(50) NULL, 
    [Chart Colors] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
));