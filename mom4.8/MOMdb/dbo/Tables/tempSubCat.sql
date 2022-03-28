CREATE TABLE [dbo].[tempSubCat] (
    [tId]       INT           IDENTITY (1, 1) NOT NULL,
    [CType]     INT           NOT NULL,
    [SubType]   VARCHAR (150) NULL,
    [SortOrder] SMALLINT      NULL
);

