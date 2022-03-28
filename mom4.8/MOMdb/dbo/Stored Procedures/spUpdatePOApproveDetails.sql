--- Exec [spUpdatePOApproveDetails] 2579,1,'Test',NULL,1 ,'',''                 
CREATE PROCEDURE [dbo].[spUpdatePOApproveDetails]                    
 @PO INT,                              
 @Status SMALLINT,                              
 @Comments NVARCHAR(200),                              
 @Signature image = NULL,                            
 @UserID INT,                
 @FilePath NVARCHAR(256),                
 @FileName NVARCHAR(75)                
AS                              
BEGIN                              
 DECLARE @SysComment VARCHAR(max),  @ApproveDate DATETIME = GETDATE()          
                           
  IF(@Status = 1)                              
  BEGIN                    
   SELECT                               
    @SysComment = 'Approved by ' + CAST(ISNULL(Fuser,' ') AS VARCHAR(50))           
   + ' '     
   +  FORMAT( @ApproveDate, 'MM/dd/yyyy hh:mm tt', 'en-us')   
   + ' - ' + @Comments               
   FROM tblUser u             
   WHERE u.ID = @UserID    
   
  -- select * from tbluser Where id = 1           
              
   IF EXISTS (SELECT PO FROM  ApprovalStatus WHERE PO = @PO  )        
   BEGIN        
    UPDATE ApprovalStatus          
    SET Status = @Status ,Comments = @Comments ,Signature =  @Signature ,ApproveDate = @ApproveDate ,UserID =    @UserID                     
    WHERE PO = @PO        
   END        
   ELSE        
   BEGIN        
    INSERT INTO ApprovalStatus  (PO,Status,Comments,Signature,ApproveDate,UserID)                            
    VALUES(@PO,@Status,@SysComment,@Signature,@ApproveDate ,@UserID)           
   END                        
                 
   ---- ES-1757                     
   ---- Update PO SET Custom1 = Convert(varchar(50),@SysComment) WHERE PO = @PO              
              
 END                              
 ELSE                              
 BEGIN              
   SELECT                               
    @SysComment = 'Declined by ' + CAST(ISNULL(fUser,' ') AS VARCHAR(50))            
     + ' '      
     +  FORMAT( @ApproveDate, 'MM/dd/yyyy hh:mm tt', 'en-us') 
	 + ' - ' + @Comments     
   FROM tblUser u             
   WHERE u.ID = @UserID         
               
   IF EXISTS (SELECT PO FROM  ApprovalStatus WHERE PO = @PO  )        
   BEGIN        
    UPDATE ApprovalStatus          
		 SET Status = @Status ,Comments = @Comments ,
			Signature =  @Signature ,
			ApproveDate = @ApproveDate ,UserID =    @UserID                     
    WHERE PO = @PO        
   END        
   ELSE        
   BEGIN        
     INSERT INTO ApprovalStatus  (PO,Status,Comments,Signature,ApproveDate,UserID)                              
     VALUES(@PO,@Status,@SysComment,@Signature,@ApproveDate ,@UserID)        
   END          
                 
 END               
         
 Update PO SET fDesc =  @SysComment + ' '  +  fDesc   WHERE PO = @PO          
          
           
  DECLARE @Line INT                 
  SET @Line = ISNULL((SELECT MAX(Line) FROM Documents WHERE ScreenID = @PO),0) + 1                
                
  INSERT INTO Documents(Screen,ScreenID,Line,fDesc,Filename,Path,Type)                
  VALUES('PO',@PO,@Line,@SysComment,@FileName,@FilePath,3)                
                              
  Exec [spGetApprovalStatusDetailsForReport] @PO                           
END  