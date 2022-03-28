
CREATE TRIGGER [dbo].[trgJobTItemUpdate] ON [dbo].[JobTItem] 

AFTER   UPDATE   

AS

BEGIN

      INSERT INTO JobTItem_Log ( 
	        [Date]
	       ,[ID]
	       ,[JobT]
           ,[Job]
           ,[Type]
           ,[fDesc]
           ,[Code]
           ,[Actual]
           ,[Budget]
           ,[Line]
           ,[Percent]
           ,[Comm]
           ,[Stored]
           ,[Modifier]
           ,[ETC]
           ,[ETCMod]
           ,[THours]
           ,[FC]
           ,[Labor]
           ,[BHours]
           ,[GL]
           ,[OrderNo]
           ,[GroupID]
           ,[TargetHours]
           ,[GanttTaskID]
           ,[EstConvertId]
           ,[EstConvertLine]
           )
        SELECT 
			GETDATE()
		   ,d.[ID]
	       ,d.[JobT]
           ,d.[Job]
           ,d.[Type]
           ,d.[fDesc]
           ,d.[Code]
           ,d.[Actual]
           ,d.[Budget]
           ,d.[Line]
           ,d.[Percent]
           ,d.[Comm]
           ,d.[Stored]
           ,d.[Modifier]
           ,d.[ETC]
           ,d.[ETCMod]
           ,d.[THours]
           ,d.[FC]
           ,d.[Labor]
           ,d.[BHours]
           ,d.[GL]
           ,d.[OrderNo]
           ,d.[GroupID]
           ,d.[TargetHours]    
           ,d.[GanttTaskID]
           ,d.[EstConvertId]
           ,d.[EstConvertLine]
	   FROM inserted i
       FULL JOIN deleted  d on d.id = i.id

END
