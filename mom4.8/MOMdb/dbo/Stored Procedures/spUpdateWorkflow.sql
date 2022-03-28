CREATE PROC  [dbo].[spUpdateWorkflow]   
	@WorkflowItem   tblTypeWorkflowItem readonly,
	@DeleteWorkflowItem  tblTypeWorkflowItem readonly,    
    @Workflow AS tblTypeWorkflow readonly  
AS    
BEGIN    
DECLARE @ID int 
DECLARE @tblWorkflowFieldsId int 
DECLARE @Label VARCHAR(50)
DECLARE @Line smallint 
DECLARE @OrderNo int 
DECLARE @IsAlert bit 
DECLARE @TeamMember VARCHAR(500)
DECLARE @Format smallint 
DECLARE @TeamMemberDisplay VARCHAR(MAX)


SET NOCOUNT ON;    

BEGIN TRANSACTION   

 DECLARE db_cursor1 CURSOR FOR      
	SELECT [ID],[Line],[OrderNo],[Label] ,[IsAlert],[TeamMember],[Format],TeamMemberDisplay FROM @DeleteWorkflowItem        
 OPEN db_cursor1      
 FETCH NEXT FROM db_cursor1 INTO @ID, @Line, @OrderNo ,   @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay
    
WHILE @@FETCH_STATUS = 0    
	 BEGIN     
		  DELETE FROM [dbo].[tblWorkflowFields]
		  WHERE   ID = @ID 

		  DELETE FROM  tblWorkflow
		  WHERE  tblWorkflowFieldsID=@ID
		  FETCH NEXT FROM db_cursor1 INTO @ID, @Line, @OrderNo ,  @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay
	 END 

CLOSE db_cursor1    
DEALLOCATE db_cursor1 

       
DECLARE db_cursor2 CURSOR FOR     
    
	SELECT [ID],[Line],[OrderNo],[Label] ,[IsAlert],[TeamMember],[Format],TeamMemberDisplay FROM @WorkflowItem   
	WHERE Label<>''
    
OPEN db_cursor2      
FETCH NEXT FROM db_cursor2 INTO @ID, @Line, @OrderNo , @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay
	
 WHILE @@FETCH_STATUS = 0    
	 BEGIN     
		  IF(SELECT TOP 1  1 FROM [dbo].[tblWorkflowFields] WHERE ID = @ID) = 1   
			  BEGIN   	  
					UPDATE [dbo].[tblWorkflowFields]
					   SET [Line] = @Line
						  ,[OrderNo] =@OrderNo
						  ,[Label] = @Label
						  ,[IsAlert] =@IsAlert
						  ,[TeamMember] = @TeamMember
						  ,[TeamMemberDisplay] = @TeamMemberDisplay						 
						  ,[Format] =@Format
					 WHERE ID=@ID

					  IF @@ERROR <> 0 AND @@TRANCOUNT > 0    
					   BEGIN      
						RAISERROR ('Error Occured', 16, 1)      
						 ROLLBACK TRANSACTION        
						RETURN    
					   END    
					   -- Update value for Test Custom Value

					     DELETE FROM [tblWorkflow] WHERE [tblWorkflowFieldsID] = @ID 
						   INSERT INTO [dbo].[tblWorkflow]([tblWorkflowFieldsID],[Line],[Value])    
					   SELECT @ID, Line, Value FROM @Workflow WHERE [Line] = @Line  
					 
			  END    
		  ELSE
			   BEGIN
					INSERT INTO [dbo].[tblWorkflowFields]
					   ([Line],[OrderNo],[Label],[IsAlert],[TeamMember],[Format],[TeamMemberDisplay])
					 VALUES
					   (@Line,@OrderNo,@Label,@IsAlert,@TeamMember,@Format,@TeamMemberDisplay)

						--	   -- Get new ID
					 SET @tblWorkflowFieldsId=SCOPE_IDENTITY()    
    
					  IF @@ERROR <> 0 AND @@TRANCOUNT > 0    
					   BEGIN      
						RAISERROR ('Error Occured', 16, 1)      
						 ROLLBACK TRANSACTION        
						RETURN    
					   END    

					   INSERT INTO [dbo].[tblWorkflow]([tblWorkflowFieldsID],[Line],[Value])    
					   SELECT @tblWorkflowFieldsId, Line, Value FROM @Workflow WHERE Line = @Line    
 
			   END
			   FETCH NEXT FROM db_cursor2 INTO @ID, @Line, @OrderNo , @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay
	 END
 CLOSE db_cursor2    
 DEALLOCATE db_cursor2 
      
    
 COMMIT TRANSACTION    
    
END