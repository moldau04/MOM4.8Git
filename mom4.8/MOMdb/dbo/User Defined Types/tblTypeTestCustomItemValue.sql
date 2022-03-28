CREATE TYPE [dbo].[tblTypeTestCustomItemValue] AS TABLE (
	[ID] [int] NULL,
	[tblTestCustomFieldsID] [int] NOT NULL,
	[Value] [varchar](Max) NULL,
	[UpdatedBy] [varchar](50) NULL,
	[TestID] [int] NOT NULL,
	[EquipmentID] [int] NOT NULL,
	[IsAlert] [bit] NULL,
	[TeamMember] [varchar](max) NULL,
	[TeamMemberDisplay] [varchar](max) NULL,
	[UserRoles] [varchar](max) NULL,
	[UserRolesDisplay] [varchar](max) NULL
	)
	

