--EXEC spDeleteWIP 22 , 36  
CREATE PROCEDURE spDeleteWIP      
@JobId INT,        
@WIPId INT = NULL        
AS                  
BEGIN                  
                  
 SET NOCOUNT ON;     
   
 -- Delete invoice if it is created    
 DECLARE @Ref INT,      
 @Batch INT,      
 @Loc INT      
    
 SELECT     
 @Ref = InvoiceId,    
 @Batch = I.Batch,    
 @Loc = I.Loc    
 FROM WIPHeader  AS W    
 INNER JOIN Invoice AS I ON I.Ref = W.InvoiceId    
 WHERE Id = @WIPId            
        
 IF(@Ref IS NOT NULL)    
 BEGIN    
  -- 'Invoice found, deleting it!'    
  EXEC [spDeleteInvoice]  @ref, @Batch, @loc    
 END    
  
 DELETE FROM WD       
 FROM WIPDetails AS WD      
 WHERE (WD.WIPId = @WIPId)      
                
 DELETE FROM W       
 FROM WIPHeader AS W                
 WHERE (W.Id = @WIPId)      
   
 Select @WIPId      
END