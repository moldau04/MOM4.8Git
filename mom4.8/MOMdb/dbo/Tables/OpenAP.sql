CREATE TABLE [dbo].[OpenAP] (
    [Vendor]     INT             NULL,
    [fDate]      DATETIME        NULL,
    [Due]        DATETIME        NULL,
    [Type]       SMALLINT        NULL,
    [fDesc]      VARCHAR (255)   NULL,
    [Original]   NUMERIC (30, 2) NULL,
    [Balance]    NUMERIC (30, 2) NULL,
    [Selected]   NUMERIC (30, 2) CONSTRAINT [DF_OpenAP_Selected] DEFAULT ((0)) NOT NULL,
    [Disc]       NUMERIC (30, 2) CONSTRAINT [DF_OpenAP_Disc] DEFAULT ((0)) NOT NULL,
    [PJID]       INT             NULL,
    [TRID]       INT             NULL,
    [Ref]        VARCHAR (50)    NULL,
    [IsSelected] BIT             NULL
);




GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_OpenAP_Vendor]
    ON [dbo].[OpenAP]([Vendor] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_OpenAP_TRID]
    ON [dbo].[OpenAP]([TRID] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_OpenAP_Ref]
    ON [dbo].[OpenAP]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_OpenAP_PJID]
    ON [dbo].[OpenAP]([PJID] DESC);

