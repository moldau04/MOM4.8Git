   -- Usp_SubstantialCompletionFinalNotPaidReport                    
CREATE PROCEDURE [dbo].[Usp_SubstantialCompletionFinalNotPaidReport]                   
AS                  
BEGIN       
                 
  SELECT                  
   j.ID ProjectId, ro.Phone MainPhoneNumber,  ro.Cellular CustomerCell,                  
   (SELECT top 1 tag    FROM   Loc  WHERE  Loc = j.Loc) ProjectLocation ,  
     (SELECT top 1 City    FROM   Loc  WHERE  Loc = j.Loc) ProjectCity ,  
   (select top 1 name from rol where id=(select top 1 rol from owner where id= j.owner)) CustomerName ,                        
    vw.[Unit Substantially Complete]    UnitSubstantiallyComplete     
   ,vw.[PAYMENT 3 - DELIVERY PYMT RCVD]    Payment3  
   ,vw.[PAYMENT 4 - FINAL PYMT RCVD]  Payment4   
    FROM   Job j                   
     INNER JOIN Elev e ON e.Loc = j.loc            
 LEFT JOIN ROL ro ON ro.ID = j.Rol                                  
 LEFT JOIN JobT jt ON j.template=jt.ID        
 JOIN  [vw_OpenJobReport] vw  ON vw.[Project #] = j.ID      
 Where isnull(vw.[Unit Substantially Complete],'')   != '' and  isnull(vw.[PAYMENT 4 - FINAL PYMT RCVD],'')  =''      
 order By j.ID desc       
                      
                  
 END 