CREATE PROCEDURE spAddWIPHeader          
@Id      INT          
,@JobId     INT          
,@ProgressBillingNo  NVARCHAR(200)          
,@InvoiceId    INT = NULL          
,@BillingDate   DATETIME          
,@ApplicationStatusId INT          
,@Terms     INT          
,@SalesTax    numeric(30,4)        
,@PeriodDate   DATETIME  
,@RevisionDate   DATETIME  
AS          
BEGIN          
           
 IF(@Id IS NULL)          
 BEGIN          
  INSERT INTO [dbo].[WIPHeader]          
      (          
      [JobId]          
      ,[ProgressBillingNo]          
      ,[InvoiceId]          
      ,[BillingDate]          
      ,[ApplicationStatusId]          
      ,[Terms]          
      ,[SalesTax]          
      ,[CreatedDate]          
      ,[LastUpdateDate]
	  ,[PeriodDate]
	  ,[RevisionDate]
      )          
   VALUES          
    (          
    @JobId          
      ,@ProgressBillingNo          
      ,@InvoiceId          
      ,@BillingDate          
      ,1 -- @ApplicationStatusId          
      ,@Terms          
      ,@SalesTax          
      ,GetDate()          
      ,GetDate()      
	  ,@PeriodDate
	  ,@RevisionDate
      )          
   Select SCOPE_IDENTITY()          
          
 END          
 ELSE          
 BEGIN          
  UPDATE [dbo].[WIPHeader]          
     SET [ProgressBillingNo] = @ProgressBillingNo          
     ,[BillingDate] = @BillingDate          
     ,[Terms] = @Terms          
     ,[SalesTax] = @SalesTax          
     ,[LastUpdateDate] = GETDATE()          
   WHERE           
   Id = @Id          
          
   Select @Id          
 END          
END