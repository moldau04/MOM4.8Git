CREATE TABLE [dbo].[Photo] (
    [ID]        INT           NOT NULL,
    [Screen]    VARCHAR (20)  NULL,
    [ScreenID]  INT           NULL,
    [Line]      SMALLINT      NULL,
    [Photo]     IMAGE         NULL,
    [PhotoPath] VARCHAR (255) NULL
);

