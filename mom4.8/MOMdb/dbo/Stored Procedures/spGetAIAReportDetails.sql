--EXEC spGetAIAReportDetails 17282,103     
CREATE PROCEDURE spGetAIAReportDetails                    
@JobId INT,                                          
@WIPID INT                        
AS                                                    
BEGIN                                                    
                      
 SELECT                                
  ROW_NUMBER() OVER(ORDER BY Line) AS RowNo,                            
  WD.Description,                            
  ScheduledValues,                      
  PreviousBilled = PreviousBilled, --+ (Select SUM(RetainageAmount) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = W.JobId AND T1.Line = WD.Line)-RetainageAmount,      
  CompletedThisPeriod,                            
  MaterialsPresentlyStored = PresentlyStored,                            
  TotalCompletedAndStored = CASE WHEN TotalCompletedAndStored > ScheduledValues THEN ScheduledValues ELSE TotalCompletedAndStored END,                   
  PerComplete = CASE WHEN TotalCompletedAndStored > ScheduledValues THEN 100 ELSE CASE WHEN ScheduledValues > 0 THEN CAST(((TotalCompletedAndStored / ScheduledValues)*100) AS DECIMAL(18,2)) else NULL END END,  
  BalanceToFinsh,                            
  Retainage = CASE WHEN TotalCompletedAndStored > ScheduledValues THEN 0 ELSE (Select SUM(RetainageAmount) FROM WIPHeader AS T INNER JOIN WIPDetails AS T1 ON T.Id = T1.WIPId WHERE T.JobId = W.JobId AND T1.Line = WD.Line AND t1.WIPId <= @WIPID) END,  
  Line            
 FROM WIPHeader AS W                                                  
 INNER JOIN WIPDetails AS WD ON WD.WIPId = W.ID                            
 WHERE W.JobId = @JobId  AND W.ID = @WIPID          
 ORDER BY  Line      
                              
END