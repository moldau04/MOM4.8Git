CREATE TRIGGER trgTicketDPDA ON TicketDPDA
AFTER DELETE  
AS
BEGIN

      INSERT INTO [TicketDPDA_Log] ( [TicketID]    ,[Date] , [data])
      SELECT  t1.ID  , (GETDATE()) ,  ( SELECT   [ID]
      ,[CDate] ,[DDate] ,[EDate] ,[fWork]     ,[Job]       ,[Loc] ,[Elev] ,[Type]  ,[Charge]       
      ,[ClearCheck]    ,[ClearPR] ,[Total]    ,[Reg]       ,[OT]   ,[DT]  ,[TT]  ,[Zone]  ,[Toll]
      ,[OtherE]    ,[Status]  ,[Invoice]      ,[Level]     ,[Est]  ,[Cat] ,[Who] ,[fBy]   ,[SMile] ,[EMile] ,[fLong]
      ,[Latt] ,[WageC] ,[Phase]   ,[Car]      ,[CallIn]    ,[Mileage] ,[NT]  ,[CauseID] ,[CauseDesc]  ,[fGroup] ,[PriceL]
      ,[WorkOrder] ,[TimeRoute]   ,[TimeSite] ,[TimeComp]  ,[Source]  ,[Internet]    ,[Custom1]
      ,[Custom2]   ,[Custom3]     ,[Custom4]  ,[Custom5]       ,[BRemarks]  ,[WorkComplete]    
      ,[AID]     ,[Custom6] ,[Custom7]  ,[Custom8] ,[Custom9] ,[Custom10] ,[CPhone]
      ,[RegTrav] ,[OTTrav]    ,[DTTrav] ,[NTTrav]  ,[Email]     ,[QBInvoiceID]   ,[break_time]   ,[Comments]
      ,[PartsUsed]  FROM deleted where id=t1.ID  FOR XML AUTO)   
	   FROM deleted t1

END;


 
 
       