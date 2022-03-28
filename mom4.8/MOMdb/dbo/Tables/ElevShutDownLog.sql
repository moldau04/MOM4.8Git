CREATE TABLE [dbo].[ElevShutDownLog](
		[id]			INT				IDENTITY(1,1) NOT NULL,
		[ticket_id]		INT				NULL,
		[status]		INT				NULL,
		[elev_id]		INT				NULL,
		[created_on]	DATETIME		NULL,
		[created_by]	INT				NULL,
		[reason]		VARCHAR(MAX)	NULL, 
		[longdesc]		VARCHAR(MAX)	NULL,
		[planned]		Bit				NULL, 
    CONSTRAINT [PK_ElevShutDownLog] PRIMARY KEY ([id])
	) ON [PRIMARY]