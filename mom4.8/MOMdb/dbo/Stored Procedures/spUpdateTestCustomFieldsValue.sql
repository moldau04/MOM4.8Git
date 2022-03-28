CREATE PROC  [dbo].[spUpdateTestCustomFieldsValue]  
@TestID int,
@EquipmentID int,
@TestCustomItemValue   tblTypeTestCustomItemValue readonly
AS
Begin
DECLARE @ID int
DECLARE @tblTestCustomFieldsID int
DECLARE @Value varchar(50)
DECLARE @UpdatedBy varchar(50)
DECLARE @IsAlert bit
DECLARE @TeamMember varchar(max)
DECLARE @TeamMemberDisplay VARCHAR(MAX)
DECLARE @UserRoles VARCHAR(500)
DECLARE @UserRolesDisplay VARCHAR(MAX)
DECLARE @label VARCHAR(MAX)
DECLARE @oldValue VARCHAR(MAX)
SET NOCOUNT ON;    

BEGIN TRANSACTION 
 DECLARE db_cursor1 CURSOR FOR      
	SELECT [ID],[tblTestCustomFieldsID],[Value],[UpdatedBy],[IsAlert],[TeamMember],TeamMemberDisplay,UserRoles,UserRolesDisplay FROM @TestCustomItemValue        
 OPEN db_cursor1      
 FETCH NEXT FROM db_cursor1 INTO @ID, @tblTestCustomFieldsID,@Value, @UpdatedBy,@IsAlert,@TeamMember,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay
    WHILE @@FETCH_STATUS = 0    
	 BEGIN  
	  if Not EXISTS (select 1 from [tblTestCustomFieldsValue] where [TestID]=@TestID and [EquipmentID]= @EquipmentID and [tblTestCustomFieldsID]= @tblTestCustomFieldsID)
		  BEGIN
			  IF (@Value<>'')
				  BEGIN
					INSERT INTO [tblTestCustomFieldsValue]
				   ([TestID]
				   ,[EquipmentID]
				   ,[tblTestCustomFieldsID]
				   ,[Value]
				   ,[UpdatedBy]
				   ,[UpdatedDate]
				   ,IsAlert
				   ,TeamMember,TeamMemberDisplay,UserRoles,UserRolesDisplay)
				 VALUES
				   (@TestID
				   ,@EquipmentID
				   ,@tblTestCustomFieldsID
				   ,@Value
				   ,@UpdatedBy
				   ,GETDATE()
				   ,@IsAlert
				   ,@TeamMember,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay)		   
			

					SELECT @label=Label
					FROM tblTestCustomFields tb
					WHERE ID=@tblTestCustomFieldsID

					 exec log2_insert @UpdatedBy,'SafetyTest',@TestID,@label,'',@Value
				  END
		  END
		  
	  Else
		  BEGIN

		   -- Update information for Rol and TeamMember
		    UPDATE [tblTestCustomFieldsValue]
			   SET 
				  [EquipmentID] = @EquipmentID				
				  ,[IsAlert] = @IsAlert
				  ,[TeamMember] = @TeamMember
				  ,[TeamMemberDisplay]=@TeamMemberDisplay
				  ,[UserRoles]=@UserRoles
				  ,[UserRolesDisplay]=@UserRolesDisplay
			 WHERE [TestID]=@TestID and [EquipmentID]= @EquipmentID and [tblTestCustomFieldsID]= @tblTestCustomFieldsID

           if Not EXISTS (select 1 from [tblTestCustomFieldsValue] where [TestID]=@TestID and [EquipmentID]= @EquipmentID and [tblTestCustomFieldsID]= @tblTestCustomFieldsID and [Value] =@Value)
		   BEGIN

		    SET @oldValue=ISNULL((select [Value] from [tblTestCustomFieldsValue] where [TestID]=@TestID and [EquipmentID]= @EquipmentID and [tblTestCustomFieldsID]= @tblTestCustomFieldsID),'')
		    UPDATE [tblTestCustomFieldsValue]
			   SET 
				  [EquipmentID] = @EquipmentID
				  ,[tblTestCustomFieldsID] = @tblTestCustomFieldsID
				  ,[Value] = @Value
				  ,[UpdatedBy] = @UpdatedBy
				  ,[UpdatedDate] = GETDATE()
				  ,[IsAlert] = @IsAlert
				  ,[TeamMember] = @TeamMember
				  ,[TeamMemberDisplay]=@TeamMemberDisplay
				  ,[UserRoles]=@UserRoles
				  ,[UserRolesDisplay]=@UserRolesDisplay
			 WHERE [TestID]=@TestID and [EquipmentID]= @EquipmentID and [tblTestCustomFieldsID]= @tblTestCustomFieldsID

			 SELECT @label=Label
					FROM tblTestCustomFields tb
					WHERE ID=@tblTestCustomFieldsID

					 exec log2_insert @UpdatedBy,'SafetyTest',@TestID,@label,@oldValue,@Value
           End
			 
		  End
		  
			

		 FETCH NEXT FROM db_cursor1 INTO @ID, @tblTestCustomFieldsID,@Value, @UpdatedBy,@IsAlert,@TeamMember,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay
	 END 

CLOSE db_cursor1    
DEALLOCATE db_cursor1 
 COMMIT TRANSACTION   
END

