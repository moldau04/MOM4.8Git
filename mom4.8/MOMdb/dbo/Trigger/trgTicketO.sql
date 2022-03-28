CREATE TRIGGER trgTicketO ON TicketO
AFTER DELETE  
AS
BEGIN

      INSERT INTO [TicketO_Log] ( [TicketID]    ,[Date] , [data])
      SELECT  t1.ID  , (GETDATE()) ,  ( SELECT   [ID]
      ,[CDate] ,[DDate] ,[EDate] ,[fWork]     ,[Job]  ,LID  
      , [Level]     ,[Est]  ,[Cat] ,[Who] ,[fBy]   ,[SMile] ,[EMile] ,[fLong]
      ,[Latt]   ,[CallIn]    ,   [fGroup] ,[PriceL]
      ,[WorkOrder] ,[TimeRoute]   ,[TimeSite] ,[TimeComp]  ,[Source]   ,[Custom1]
      ,[Custom2]   ,[Custom3]     ,[Custom4]  ,[Custom5]       ,[BRemarks]  ,  
       [AID]     ,[Custom6] ,[Custom7]  ,[Custom8] ,[Custom9] ,[Custom10] ,[CPhone] 
       FROM deleted where id=t1.ID  FOR XML AUTO)   
	   FROM deleted t1

END;


 
 
       