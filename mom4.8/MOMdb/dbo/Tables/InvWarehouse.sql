CREATE TABLE [dbo].[InvWarehouse](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InvID] [int] NULL,
	[WareHouseID] [varchar](5) NULL,
 CONSTRAINT [PK_InvWarehouse] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
