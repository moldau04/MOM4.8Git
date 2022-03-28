CREATE TABLE [dbo].[JobNotes] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
	[JobID]	 INT NOT NULL,
    [Note]      VARCHAR (MAX) NULL,	
    [CreatedDate] DATETIME,	
	[CreatedBy] int
    CONSTRAINT [PK_JobNotes] PRIMARY KEY ([ID])
);