CREATE TABLE [dbo].[tblTestCustomFieldsValue](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TestID] [int] NOT NULL,
	[EquipmentID] [int] NOT NULL,
	[tblTestCustomFieldsID] [int] NOT NULL,
	[Value] [varchar](MAX) NULL,
	[UpdatedBy] [varchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
	[IsAlert] [bit] NULL,
	[TeamMember] [varchar](Max) NULL,
	[TeamMemberDisplay] [varchar](Max) NULL,
	[UserRoles] [varchar](MAX) NULL,
	[UserRolesDisplay] [varchar](Max) NULL,
	[TestYear] int
 CONSTRAINT [PK_tblTestCustomFieldsValue] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO