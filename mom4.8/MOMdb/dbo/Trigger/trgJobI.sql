Create TRIGGER [dbo].[trgJobI] ON [dbo].[JobI] 

AFTER   DELETE   

AS

BEGIN

   INSERT INTO [dbo].[JobI_Log]
           ([Date]
           ,[Job]
           ,[Phase]
           ,[fDate]
           ,[Ref]
           ,[fDesc]
           ,[Amount]
           ,[TransID]
           ,[Type]
           ,[Labor]
           ,[Billed]
           ,[Invoice]
           ,[UseTax]
           ,[APTicket])
		   select 
		   getdate()
		   ,[Job]
           ,[Phase]
           ,[fDate]
           ,[Ref]
           ,[fDesc]
           ,[Amount]
           ,[TransID]
           ,[Type]
           ,[Labor]
           ,[Billed]
           ,[Invoice]
           ,[UseTax]
           ,[APTicket]
		   FROM deleted t1

END
