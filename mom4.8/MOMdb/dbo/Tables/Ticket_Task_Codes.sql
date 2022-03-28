CREATE TABLE [dbo].[Ticket_Task_Codes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ticket_id] [int] NULL,
	[task_code] [varchar](200) NULL, 
    [Category] VARCHAR(100) NULL, 
    [Type] SMALLINT NULL, 
    [job] INT NULL, 
    [username] VARCHAR(50) NULL, 
    [dateupdated] DATETIME NULL, 
    [default_code] SMALLINT NULL
) ON [PRIMARY]
