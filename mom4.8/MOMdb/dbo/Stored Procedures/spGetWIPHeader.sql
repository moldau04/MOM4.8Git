--EXEC spGetWIPHeader 1630,65  
CREATE PROCEDURE [dbo].[spGetWIPHeader]                    
@JobId INT,            
@WIPId INT = NULL            
AS                      
BEGIN                   
                      
 SET NOCOUNT ON;
 SELECT       
 W.Id                    
 ,JobId                    
 ,ProgressBillingNo                    
 ,w.InvoiceId                    
 ,ISNULL(BillingDate,GETDATE()) AS BillingDate                    
 ,ISNULL(ApplicationStatusId,1) AS ApplicationStatusId          
 ,ApplicationStatusIdText = (Select TOP 1 StatusName from ApplicationStatus WHERE ID = ApplicationStatusId)              
 ,w.Terms AS Terms              
 ,w.SalesTax
 --,ArchitectName                    
 --,ArchitectAddress                    
 ,w.CreatedDate                    
 ,W.LastUpdateDate              
 ,Amount = (SELECT SUM(TotalBilled) FROM WIPDetails AS WD WHERE WD.WIPId = W.ID)              
 ,SubmittedDate = FORMAT(W.LastUpdateDate, 'M/d/yyyy')        
 ,Approvedby = (Select fUser from Invoice where Ref = W.InvoiceId)                   
 ,SendTo              
 ,SendBy   
 ,SendOn = FORMAT(W.SendOn, 'M/d/yyyy hh:mm') 
 ,ISNULL(PeriodDate,GETDATE()) AS PeriodDate
 ,ISNULL(RevisionDate,GETDATE()) AS RevisionDate
 ,ISNULL(i.Total,0) AS InvoiceAmount
  ,ISNULL(i.Amount,0) AS InvoicePreTax
 ,ISNULL(i.Status,0) AS InvoiceStatus
 ,ISNULL(ar.Balance,0) AS InvoiceAmountDue
  ,ISNULL(i.STax,0) AS PstAmount
  ,ISNULL((select sum(GstAmount) from InvoiceI where INvoiceI.Ref=I.Ref group by InvoiceI.Ref),0) as GstAmount
  ,ISNULL(i.fDesc,'') invoiceDescription 
  FROM WIPHeader AS W                    
  INNER JOIN Job AS J ON J.ID = W.JobId  
  LEFT JOIN Invoice AS i ON I.Ref = isnull(w.InvoiceId,0)
  LEFT JOIN OpenAR  AS ar on ar.Ref=I.Ref
  WHERE J.ID = @JobId                   
  AND (W.Id = @WIPId OR @WIPId IS NULL) 
  ORDER BY W.id
                  
  EXEC [spGetWIPDetails] @WIPId,@JobId  
  
  declare @GSTRate numeric(30,2) = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
							THEN 
								CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
							ELSE 
								0.00
							END
								AS GSTRate),0)

 declare @Type INT = ISNULL((SELECT top 1 STax.Type as Type FROM Loc l INNER JOIN STax ON STax.name = l.stax WHERE l.loc=(SELECT loc FROM Job WHERE ID=@JobId)),0)
 SELECT @GSTRate AS GSTRate, @Type AS TaxType
END