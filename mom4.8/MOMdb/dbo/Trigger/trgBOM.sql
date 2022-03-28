CREATE TRIGGER [dbo].[trgBOM] ON [dbo].[BOM] 

AFTER   DELETE   

AS

BEGIN

      INSERT INTO BOM_Log ( 
	    [Date]
	   ,[ID]
      ,[JobTItemID]
      ,[Type]
      ,[Item]
      ,[QtyRequired]
      ,[UM]
      ,[ScrapFactor]
      ,[BudgetUnit]
      ,[BudgetExt]
      ,[Vendor]
      ,[Currency]
      ,[EstimateIId]
      ,[MatItem]
      ,[LabItem]
      ,[SDate]
      ,[LabRate])
            SELECT 
			GETDATE()
	  ,[ID]
      ,[JobTItemID]
      ,[Type]
      ,[Item]
      ,[QtyRequired]
      ,[UM]
      ,[ScrapFactor]
      ,[BudgetUnit]
      ,[BudgetExt]
      ,[Vendor]
      ,[Currency]
      ,[EstimateIId]
      ,[MatItem]
      ,[LabItem]
      ,[SDate]
      ,[LabRate]  
	   FROM deleted t1

END


 

GO