
CREATE TRIGGER [dbo].[trgJobTItem] ON [dbo].[JobTItem] 

AFTER   DELETE   

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
	   FROM deleted t1

END
