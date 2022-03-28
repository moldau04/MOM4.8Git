CREATE PROC  [dbo].[spUpdateTestCustomField]   
	@TestCustomItem   tblTypeTestCustomItem readonly,
	@DeleteTestCustomItem  tblTypeTestCustomItem readonly,    
    @TestCustom AS tblTypeTestCustom readonly  
AS    
BEGIN    
DECLARE @ID int 
DECLARE @tblCustomFieldsId int 
DECLARE @Label VARCHAR(50)
DECLARE @Line smallint 
DECLARE @OrderNo int 
DECLARE @IsAlert bit 
DECLARE @TeamMember VARCHAR(500)
DECLARE @Format smallint 
DECLARE @TeamMemberDisplay VARCHAR(MAX)
DECLARE @UserRoles VARCHAR(500)
DECLARE @UserRolesDisplay VARCHAR(MAX)


SET NOCOUNT ON;    

BEGIN TRANSACTION   

 DECLARE db_cursor1 CURSOR FOR      
	SELECT [ID],[Line],[OrderNo],[Label] ,[IsAlert],[TeamMember],[Format],TeamMemberDisplay,UserRoles,UserRolesDisplay FROM @DeleteTestCustomItem        
 OPEN db_cursor1      
 FETCH NEXT FROM db_cursor1 INTO @ID, @Line, @OrderNo ,   @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay
    
WHILE @@FETCH_STATUS = 0    
	 BEGIN     
		  DELETE FROM [dbo].[tblTestCustomFields]
		  WHERE   ID = @ID 

		  DELETE FROM  tblTestCustom
		  WHERE  tblTestCustomFieldsID=@ID
		  FETCH NEXT FROM db_cursor1 INTO @ID, @Line, @OrderNo ,  @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay
	 END 

CLOSE db_cursor1    
DEALLOCATE db_cursor1 

       
DECLARE db_cursor2 CURSOR FOR     
    
	SELECT [ID],[Line],[OrderNo],[Label] ,[IsAlert],[TeamMember],[Format],TeamMemberDisplay,UserRoles,UserRolesDisplay FROM @TestCustomItem   
	WHERE Label<>''
    
OPEN db_cursor2      
FETCH NEXT FROM db_cursor2 INTO @ID, @Line, @OrderNo , @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay
	
 WHILE @@FETCH_STATUS = 0    
	 BEGIN     
		  IF(SELECT TOP 1  1 FROM [dbo].[tblTestCustomFields] WHERE ID = @ID) = 1   
			  BEGIN   	  
					UPDATE [dbo].[tblTestCustomFields]
					   SET [Line] = @Line
						  ,[OrderNo] =@OrderNo
						  ,[Label] = @Label
						  ,[IsAlert] =@IsAlert
						  ,[TeamMember] = @TeamMember
						  ,[TeamMemberDisplay] = @TeamMemberDisplay
						  ,[UserRoles] = @UserRoles
						  ,[UserRolesDisplay] = @UserRolesDisplay
						  ,[Format] =@Format
					 WHERE ID=@ID

					  IF @@ERROR <> 0 AND @@TRANCOUNT > 0    
					   BEGIN      
						RAISERROR ('Error Occured', 16, 1)      
						 ROLLBACK TRANSACTION        
						RETURN    
					   END    
					   -- Update value for Test Custom Value

					     DELETE FROM [tblTestCustom] WHERE [tblTestCustomFieldsID] = @ID 
						   INSERT INTO [dbo].[tblTestCustom]([tblTestCustomFieldsID],[Line],[Value])    
					   SELECT @ID, Line, Value FROM @TestCustom WHERE [Line] = @Line  
					 
			  END    
		  ELSE
			   BEGIN
					INSERT INTO [dbo].[tblTestCustomFields]
					   ([Line],[OrderNo],[Label],[IsAlert],[TeamMember],[Format],[TeamMemberDisplay],[UserRoles],[UserRolesDisplay])
					 VALUES
					   (@Line,@OrderNo,@Label,@IsAlert,@TeamMember,@Format,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay)

						--	   -- Get new ID
					 SET @tblCustomFieldsId=SCOPE_IDENTITY()    
    
					  IF @@ERROR <> 0 AND @@TRANCOUNT > 0    
					   BEGIN      
						RAISERROR ('Error Occured', 16, 1)      
						 ROLLBACK TRANSACTION        
						RETURN    
					   END    

					   INSERT INTO [dbo].[tblTestCustom]([tblTestCustomFieldsID],[Line],[Value])    
					   SELECT @tblCustomFieldsId, Line, Value FROM @TestCustom WHERE Line = @Line    
 
			   END
			   FETCH NEXT FROM db_cursor2 INTO @ID, @Line, @OrderNo , @Label, @IsAlert, @TeamMember,@Format ,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay
	 END
 CLOSE db_cursor2    
 DEALLOCATE db_cursor2 
      
    
 COMMIT TRANSACTION    
    
END