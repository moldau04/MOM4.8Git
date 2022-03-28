CREATE TABLE [dbo].[tblEmail] (
    [ID]           INT              IDENTITY (1, 1) NOT NULL,
    [From]         VARCHAR (100)    NULL,
    [To]           TEXT             NULL,
    [Cc]           TEXT             NULL,
    [Bcc]          TEXT             NULL,
    [Subject]      VARCHAR (200)    NULL,
    [SentDate]     DATETIME         NULL,
    [RecDate]      DATETIME         NULL,
    [Attachments]  SMALLINT         NULL,
    [msgID]        VARCHAR (200)    NULL,
    [UID]          INT              NULL,
    [BodyReceived] SMALLINT         NULL,
    [GUID]         UNIQUEIDENTIFIER NULL,
    [Type]         SMALLINT         NULL,
    [User]         INT              NULL,
    [AccountID]    VARCHAR (100)    NULL,
    [Rol]          INT              NULL, 
    CONSTRAINT [PK_tblEmail] PRIMARY KEY ([ID])
);

