/*--------------------------------------------------------------------
Created By: Thurstan
Modified On: 28 Jan 2019	
Description: Create spConvertEquipment
--------------------------------------------------------------------*/

CREATE PROCEDURE [dbo].[spConvertEquipment]
	@Lead int null
as
BEGIN
Declare @ElevID int = 0;
Insert into Elev
([Unit]
      ,[State]
	  ,[Loc]
	  ,[Owner]
      ,[Cat]
      ,[Type]
      ,[Building]
      ,[Manuf]
      ,[Remarks]
      ,[Install]
      ,[InstallBy]
      ,[Since]
      ,[Last]
      ,[Price]
      ,[fGroup]
      ,[fDesc]
      ,[Serial]
      ,[Template]
      ,[Status]
      ,[AID]
      ,[Week]
      ,[Category]
      ,[CustomField]
      ,[PrimarySyncID]
      ,[LastUpdateDate]  
      ,[Route]
      ,[shut_down]
      ,[Classification]
      ,[ShutdownReason]

)
Select [Unit]
      ,[State]
	  ,null
	  ,null
      ,[Cat]
      ,[Type]
      ,[Building]
      ,[Manuf]
      ,[Remarks]
      ,[Install]
      ,[InstallBy]
      ,[Since]
      ,[Last]
      ,[Price]
      ,[fGroup]
      ,[fDesc]
      ,[Serial]
      ,[Template]
      ,[Status]
      ,[AID]
      ,[Week]
      ,[Category]
      ,[CustomField]
      ,[PrimarySyncID]
      ,[LastUpdateDate]
      ,[Route]
      ,[shut_down]
      ,[Classification]
      ,[ShutdownReason]
	  From LeadEquip
	  where Lead = @Lead

Set @ElevID = SCOPE_IDENTITY();

	  Update ElevTItem
Set Elev = @ElevID
Where LeadEquip = @Lead

Update ElevTItem
Set LeadEquip = null
Where LeadEquip = @Lead

Delete from LeadEquip
Where Lead = @Lead

END

