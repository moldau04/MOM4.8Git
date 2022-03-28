CREATE TABLE [dbo].[Log2] (
    [fUser]        VARCHAR (50)   NULL,
    [Screen]       VARCHAR (50)   NULL,
    [Ref]          BIGINT         NULL,
    [Field]        VARCHAR (75)   NULL,
    [OldVal]       VARCHAR (1000) NULL,
    [NewVal]       VARCHAR (1000) NULL,
    [CreatedStamp] DATETIME       CONSTRAINT [DF_Log2_CreatedStamp] DEFAULT (getdate()) NOT NULL,
    [fDate]        AS             (dateadd(day,(0),datediff(day,(0),[CreatedStamp]))) PERSISTED,
    [fTime]        AS             (dateadd(day, -datediff(day,(0),[CreatedStamp]),[CreatedStamp])) PERSISTED
);

 

GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_LOG2_Screen]
    ON [dbo].[Log2]([Screen] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_LOG2_Ref]
    ON [dbo].[Log2]([Ref] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_LOG2_fUser]
    ON [dbo].[Log2]([fUser] DESC);


GO
CREATE NONCLUSTERED INDEX [MOM_INDEX_LOG2_fDate]
    ON [dbo].[Log2]([fDate] DESC);

