CREATE PROCEDURE spGetAIAReportHeader                              
@JobId INT,                                          
@WIPID INT                      
AS   

BEGIN                                                                                                 
	DECLARE                             
	@OriginalContractAmt  NUMERIC(18,2),                            
	@NetChangeByChangeOrder  NUMERIC(18,2),                            
	@ContractSumToDate   NUMERIC(18,2),                            
	@PerOfCompeletedWork  NUMERIC(18,2),                            
	@TotalCompletedAndStored NUMERIC(18,2),                            
	@PerOfStoredMaterial  NUMERIC(18,2),                           
	@TotalRetainage NUMERIC(18,2),                          
	@TotalEarnedLessRetainage NUMERIC(18,2),                            
	@LessPreviousCertificate NUMERIC(18,2),                            
	@CurrentPaymentDue   NUMERIC(18,2),                            
	@BalanceToFinished   NUMERIC(18,2)      ,                      
	@ChangeOrderThisPeriod   NUMERIC(18,2) ,                      
	@TotalPerComplete NUMERIC(18,2),
	@RetainagePer NUMERIC(18,2),        
	@CountOfPer INT
	
	SET @CountOfPer = (Select COUNT (DISTINCT T1.RetainagePer) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = @JobId  AND t1.WIPId <= @WIPID AND T1.RetainagePer <> 0)                    
                            
	 SELECT         
		@OriginalContractAmt  = SUM(ContractAmount)        
		,@NetChangeByChangeOrder = SUM(ChangeOrder)        
		,@ContractSumToDate  =  SUM(ContractAmount) + SUM(ChangeOrder)        
		,@TotalCompletedAndStored = SUM(CASE WHEN TotalCompletedAndStored > ScheduledValues THEN ScheduledValues ELSE TotalCompletedAndStored END)        
		,@PerOfCompeletedWork  =  SUM(PerOfCompeletedWork)      
		,@PerOfStoredMaterial  =  SUM(PerOfStoredMaterial)        
		,@TotalRetainage  =  SUM(RetainageAmount)        
		,@TotalEarnedLessRetainage = SUM(CASE WHEN TotalCompletedAndStored > ScheduledValues THEN ScheduledValues ELSE TotalCompletedAndStored END) - SUM(Retainage)        
		,@LessPreviousCertificate = (SUM(TotalCompletedAndStored) -  SUM(CompletedThisPeriod)) - (SUM(Retainage) - SUM(RetainageAmount))    
		,@CurrentPaymentDue  =  SUM(CompletedThisPeriod) - SUM(RetainageAmount)    
		,@BalanceToFinished  =  SUM(BalanceToFinsh)        
		,@ChangeOrderThisPeriod =  SUM(ChangeOrder)         
		,@TotalPerComplete   =  CASE WHEN SUM(ScheduledValues) > 0 THEN CAST((SUM(TotalCompletedAndStored) / SUM(ScheduledValues)) *100 AS DECIMAL(18,2)) ELSE NULL END
		,@RetainagePer = CASE @CountOfPer WHEN 1 THEN (Select DISTINCT T1.RetainagePer FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = @JobId  AND t1.WIPId <= @WIPID AND T1.RetainagePer <> 0) ELSE 0 END                
	FROM (        
		SELECT                                
			ContractAmount,                          
			ChangeOrder,    
			TotalCompletedAndStored,         
			ScheduledValues,      
			PreviousBilled = PreviousBilled, --+ (Select SUM(RetainageAmount) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = W.JobId AND T1.Line = WD.Line AND t1.WIPId <= @WIPID)-RetainageAmount,      
			CompletedThisPeriod,                            
			PresentlyStored,                            
			PerComplete = CASE WHEN ScheduledValues > 0 THEN CAST(((TotalCompletedAndStored / ScheduledValues)*100) AS DECIMAL(18,2)) else NULL END,                               
			BalanceToFinsh,      
			PresentlyStored as MaterialsPresentlyStored,    
			Retainage = (Select SUM(RetainageAmount) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = W.JobId AND T1.Line = WD.Line AND t1.WIPId <= @WIPID),    
			RetainageAmount,
			PerOfCompeletedWork = CASE WHEN TotalCompletedAndStored > ScheduledValues THEN 0 ELSE (Select SUM(T1.RetainageAmount - T1.PresentlyStored * T1.RetainagePer / 100) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = W.JobId AND T1.Line = WD.Line AND t1.WIPId <= @WIPID) END,
			PerOfStoredMaterial = CASE WHEN TotalCompletedAndStored > ScheduledValues THEN 0 ELSE (Select SUM(T1.PresentlyStored * T1.RetainagePer / 100) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = W.JobId AND T1.Line = WD.Line AND t1.WIPId <= @WIPID) END
		FROM WIPHeader AS W                                                  
			INNER JOIN WIPDetails AS WD ON WD.WIPId = W.ID                            
		WHERE W.JobId = @JobId  AND W.ID = @WIPID          
	) AS T         
       
                         
	SELECT DISTINCT                    
		l.Loc,                    
		J.fDesc AS ProjName,                              
		o.Type AS OwnerType,                              
		r.Name AS CustomerName,                          
		ISNULL(r.Address,'') + ', ' + ISNULL(R.City,'') + ', ' + ISNULL(r.State,'') + ', ' + ISNULL(r.Zip,'') AS OwnerAddress,                              
		ISNULL(L.Address,'') + ', ' + ISNULL(L.City,'') + ', ' + ISNULL(L.State,'') + ', ' + ISNULL(l.Zip,'') AS LocAddress,                             
		ctr.Name AS ControlName,                             
		ISNULL(ctr.Address,'') + ' ' + ISNULL(ctr.City,'') + ', ' + ISNULL(ctr.State,'') + ', ' + ISNULL(ctr.Zip,'') AS ControlAddress,                            
		ContractFOR = J.fDesc,                              
		J.ArchitectName,                              
		J.ArchitectAdress,                              
		ApplicationNo = w.ProgressBillingNo,                      
		PeriodTo = FORMAT(W.PeriodDate, 'M/d/yyyy'),  
		ProjectNos = J.ID,                              
		ContractDate = FORMAT(J.fDate, 'M/d/yyyy'), 
		BillingDate = FORMAT(ISNULL(BillingDate,GETDATE()), 'M/d/yyyy'),
		RevisionDate =FORMAT(W.RevisionDate, 'M/d/yyyy'),     
		@OriginalContractAmt AS OriginalContractAmt,                            
		@NetChangeByChangeOrder AS NetChangeByChangeOrder,                             
		@ContractSumToDate AS ContractSumToDate,                             
		@PerOfCompeletedWork AS PerOfCompeletedWork,                            
		@TotalCompletedAndStored AS TotalCompletedAndStored,                            
		@PerOfStoredMaterial AS PerOfStoredMaterial,                            
		@TotalRetainage AS TotalRetainage,                          
		@TotalEarnedLessRetainage AS TotalEarnedLessRetainage,                             
		@LessPreviousCertificate AS LessPreviousCertificate,                            
		@CurrentPaymentDue AS CurrentPaymentDue,                             
		@BalanceToFinished AS BalanceToFinished,                      
		@ChangeOrderThisPeriod  AS ChangeOrderThisPeriod ,                      
		@TotalPerComplete AS TotalPerComplete,
		@RetainagePer AS RetainagePer 
	FROM WIPHeader AS W                                                  
		INNER JOIN Job AS J ON J.ID = W.JobId                                 
		LEFT JOIN Owner o ON o.ID = j.Owner                              
		LEFT JOIN Loc l ON l.Loc=j.Loc                                      
		LEFT JOIN Rol r ON r.ID = l.Rol                                    
		CROSS JOIN [Control] as ctr                              
	WHERE J.ID = @JobId AND W.ID = @WIPID                                                
                  
	EXEC spGetAIAReportDetails @jobId, @WIPID             
END