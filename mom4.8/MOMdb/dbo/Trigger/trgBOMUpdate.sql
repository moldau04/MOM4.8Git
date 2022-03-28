CREATE TRIGGER [dbo].[trgBOMUpdate] ON [dbo].[BOM] 

AFTER   UPDATE   

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
	    ,d.[ID]
        ,d.[JobTItemID]
        ,d.[Type]
        ,d.[Item]
        ,d.[QtyRequired]
        ,d.[UM]
        ,d.[ScrapFactor]
        ,d.[BudgetUnit]
        ,d.[BudgetExt]
        ,d.[Vendor]
        ,d.[Currency]
        ,d.[EstimateIId]
        ,d.[MatItem]
        ,d.[LabItem]
        ,d.[SDate]
        ,d.[LabRate]  
	FROM inserted i
    FULL JOIN deleted  d on d.id = i.id
END
GO