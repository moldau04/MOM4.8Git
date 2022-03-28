CREATE PROCEDURE [dbo].[spAddWIPDetails]                
@JobId INT,                
@WIPId INT,            
@WIPDetails AS tblWIPDetails READONLY                
AS                  
BEGIN                  
                
 If Object_Id('tempdb.dbo.#FirstTable') Is NOT NULL                
 DROP TABLE #WIPDetails            
            
 -- Move data to temp table                
 SELECT * INTO #WIPDetails FROM @WIPDetails                
                
 -- Update if records already exist        
 UPDATE WD                
   SET                 
  [Description] = TempWD.[Description]                
    ,[ContractAmount] = TempWD.ContractAmount                
    ,[ChangeOrder] = TempWD.ChangeOrder                
    ,[ScheduledValues] = TempWD.ScheduledValues                
    ,[PreviousBilled] = TempWD.PreviousBilled                
    ,[CompletedThisPeriod] = TempWD.CompletedThisPeriod                
    ,[PresentlyStored] = TempWD.PresentlyStored                
    ,[TotalCompletedAndStored] =TempWD.TotalCompletedAndStored                
    ,[PerComplete] = TempWD.PerComplete                
    ,[BalanceToFinsh] = TempWD.BalanceToFinsh                
    ,[RetainagePer] = TempWD.RetainagePer                
    ,[RetainageAmount] = TempWD.RetainageAmount                
    ,[TotalBilled] = TempWD.TotalBilled           
 ,BillingCode = TempWD.BillingCode           
    ,[Taxable] = TempWD.Taxable                
    ,[LastUpdateDate] = GETDATE()                
  FROM WIPDetails AS WD                
  INNER JOIN #WIPDetails AS TempWD ON WD.Id = TempWD.Id    
  WHERE WD.WIPId = @WIPId  
        
  -- Delete Updated records from temp table        
  DELETE TempWD FROM WIPDetails AS WD                
  INNER JOIN #WIPDetails AS TempWD ON WD.Id = TempWD.Id      
  WHERE WD.WIPId = @WIPId   
              
  -- Insert new records         
  INSERT INTO WIPDetails                
    (                
  WIPId               
  ,Line           
  ,[Description]                
  ,ContractAmount                
  ,ChangeOrder                
  ,ScheduledValues                
  ,PreviousBilled                
  ,CompletedThisPeriod                
  ,PresentlyStored                
  ,TotalCompletedAndStored                
  ,PerComplete                
  ,BalanceToFinsh                
  ,RetainagePer                
  ,RetainageAmount                
  ,TotalBilled         
  ,BillingCode             
  ,Taxable                
  ,CreatedDate                
  ,LastUpdateDate                
    )                
    SELECT                 
   @WIPId                
    ,Line          
    ,[Description]                
    ,ContractAmount                
    ,ChangeOrder                
    ,ScheduledValues                
    ,PreviousBilled                
    ,CompletedThisPeriod                
    ,PresentlyStored                
    ,TotalCompletedAndStored                
    ,PerComplete                
    ,BalanceToFinsh                
    ,RetainagePer                
    ,RetainageAmount                
    ,TotalBilled       
 ,BillingCode               
    ,Taxable                
    ,GETDATE()                
    ,GETDATE()                
  FROM #WIPDetails                
               
 SELECT @WIPId                
END