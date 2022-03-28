CREATE PROCEDURE spUpdateWIPHeader_MailFields  
@Id       NVARCHAR(200)            
,@JobId   INT            
,@SendTo  NVARCHAR(200)            
,@SendBy  NVARCHAR(200)            
,@SendOn DATETIME            
AS            
BEGIN            
             
  UPDATE [dbo].[WIPHeader]            
     SET [SendTo] = @SendTo  
     ,[SendBy] = @SendBy  
     ,[SendOn] = @SendOn  
     ,[LastUpdateDate] = GETDATE()            
   WHERE             
   InvoiceId IN (Select SplitValue from [dbo].[fnSplit](@Id, ','))            
            
   exec spGetWIPHeader  @JobId , NULL  
END