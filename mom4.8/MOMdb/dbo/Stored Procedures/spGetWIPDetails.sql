CREATE PROCEDURE [dbo].[spGetWIPDetails]          
@WIPId INT = NULL,              
@JobId INT = NULL        
AS              
BEGIN              
              
 SET NOCOUNT ON;              

 SELECT   
 RowNo = Row_number() OVER(Order by W.Id)  
 ,WD.Id          
 ,Line        
 ,WIPId          
 ,[Description] AS WIPDesc          
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
 ,WD.CreatedDate          
 ,WD.LastUpdateDate   
 ,RetainageAmount*(-1) AS RetainageCumAmount
 ,Case 
	when Taxable=1 and GSTable is null then 1 
	when Taxable=0 and GSTable is null then 0 
	when Taxable=1 and GSTable is not null then GSTable
	when Taxable=0 and GSTable is not null then GSTable
 END as GSTable
 ,0 AS PSTAmount
 ,0 AS GSTAmount
-- ,PerComplete AS PerBilled
, isnull((select (PreviousBilled/(case ContractAmount when 0 then 1 else ContractAmount end))*100 from WIPDetails 
						where id =isnull((select max(ID) from WipDetails where WipDetails.wipID=isnull(WD.WIPId,0) and WipDetails.Line=WD.line),0)),0)	 			
						AS PerBilled
  FROM WIPDetails AS WD            
  INNER JOIN WIPHeader AS W  ON W.Id = WD.WIPId        
  INNER JOIN Job AS J ON J.ID = W.JobId              
  WHERE         
  (WD.WIPId = @WIPId OR @WIPId IS NULL) AND        
  (J.ID = @JobId OR @JobId IS NULL)        
          
END