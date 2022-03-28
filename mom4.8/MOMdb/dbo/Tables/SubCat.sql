CREATE TABLE [dbo].[SubCat] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [CType]     INT           NULL,
    [SubType]   VARCHAR (150) NULL,
    [SortOrder] SMALLINT      NULL, 
    CONSTRAINT [PK_SubCat] PRIMARY KEY ([ID])
);

