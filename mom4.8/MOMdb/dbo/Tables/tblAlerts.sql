CREATE TABLE [dbo].[tblAlerts] (
    [AlertID]      INT          IDENTITY (1, 1) NOT NULL,
    [ScreenID]     INT          NULL,
    [ScreenName]   VARCHAR (50) NULL,
    [AlertCode]    VARCHAR (50) NULL,
    [AlertSubject] VARCHAR (50) NULL,
    [AlertMessage] TEXT         NULL, 
    CONSTRAINT [PK_tblAlerts] PRIMARY KEY ([AlertID])
);
