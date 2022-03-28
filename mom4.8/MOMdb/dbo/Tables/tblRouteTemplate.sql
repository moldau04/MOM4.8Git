CREATE TABLE [dbo].[tblRouteTemplate] (
    [TemplateID]   INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    [Sequence]     VARCHAR (100) NULL,
    [Remarks]      VARCHAR (250) NULL,
    [Center]       VARCHAR (250) NULL,
    [Radius]       VARCHAR (50)  NULL,
    [Worker]       INT           NULL,
    [Overlay]      VARCHAR (15)  NULL,
    [PolygonCoord] VARCHAR (MAX) NULL, 
    CONSTRAINT [PK_tblRouteTemplate] PRIMARY KEY ([TemplateID])
);

