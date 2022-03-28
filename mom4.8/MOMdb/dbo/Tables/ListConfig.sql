CREATE TABLE [dbo].[ListConfig] (
    [idListConfig] INT            IDENTITY (1, 1) NOT NULL,
    [ListName]     NVARCHAR (20)  NOT NULL,
    [ItemName]     NVARCHAR (50)  NOT NULL,
    [ItemValue]    INT            NULL,
    [ItemCode]     NVARCHAR (5)   NULL,
    [ItemDesc]     NVARCHAR (255) NULL,
    [DestTable]    NVARCHAR (50)  NULL,
    [DestField]    NVARCHAR (50)  NULL,
    [IsDefault]    BIT            CONSTRAINT [DF_ListConfig_IsDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [pk_ListConfig_idListConfig] PRIMARY KEY CLUSTERED ([idListConfig] ASC)
);

