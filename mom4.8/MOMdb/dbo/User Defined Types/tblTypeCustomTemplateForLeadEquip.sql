/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Add LeadEquip column into tblTypeCustomTemplateForLeadEquip
--------------------------------------------------------------------*/
CREATE TYPE [dbo].[tblTypeCustomTemplateForLeadEquip] AS TABLE (
	[ID] [int] NULL
	,[ElevT] [int] NULL
	,[Elev] [int] NULL
	,[fDesc] [varchar](50) NULL
	,[Line] [smallint] NULL
	,[Value] [varchar](200) NULL
	,[Format] [varchar](50) NULL
	,[LastUpdated] [varchar](25) NULL
	,[LastUpdateUser] [varchar](20) NULL
	,[OrderNo] [int] NULL
	,[LeadEquip] [int] NULL
	)
GO



