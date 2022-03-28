CREATE TABLE [dbo].[tblConvertedProspects] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [ProspectID]   INT           NULL,
    [ProspectROL]  INT           NULL,
    [ProspectName] VARCHAR (100) NULL,
    [LocationID]   INT           NULL, 
    CONSTRAINT [PK_tblConvertedProspects] PRIMARY KEY ([ID])
);

