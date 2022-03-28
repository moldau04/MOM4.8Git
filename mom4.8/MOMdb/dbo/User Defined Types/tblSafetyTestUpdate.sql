CREATE TYPE [tblSafetyTestUpdate] AS TABLE(
	[TestID] [int] NULL,
	[EquipmentID] [int] NULL,
	[TestCustomFieldID] [int] NULL,
	[CustomValue] [varchar](max) NULL,
	[CustomOldValue] [varchar](max) NULL
)