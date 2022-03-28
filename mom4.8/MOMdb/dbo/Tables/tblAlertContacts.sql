CREATE TABLE [dbo].[tblAlertContacts] (
    [ID]         INT          IDENTITY (1, 1) NOT NULL,
    [ScreenID]   INT          NULL,
    [ScreenName] VARCHAR (50) NULL,
    [AlertID]    INT          NULL,
    [Email]      BIT          NULL,
    [Text]       BIT          NULL, 
    CONSTRAINT [PK_tblAlertContacts] PRIMARY KEY ([ID])
);