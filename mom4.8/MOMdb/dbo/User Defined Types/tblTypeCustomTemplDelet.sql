/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Add LeadEquip column into tblTypeCustomTemplDelet
--------------------------------------------------------------------*/
CREATE TYPE [dbo].[tblTypeCustomTemplDelet] AS TABLE(
	[ID] [int] NULL,
	[ElevT] [int] NULL,
	[Elev] [int] NULL,
	[fDesc] [varchar](50) NULL,
	[Line] [smallint] NULL,
	[Value] [varchar](50) NULL,
	[Format] [varchar](50) NULL,
	[OrderNo] [int] NULL,
	[LeadEquip] [int] NULL
)
GO


