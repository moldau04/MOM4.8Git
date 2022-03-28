CREATE PROCEDURE [dbo].[spUpdateListTestCustomFieldValue]  
@tblSafetyTestUpdate  tblSafetyTestUpdate readonly,
@UpdatedBy VARCHAR(800)
AS
Begin
DECLARE @c_TestID int
DECLARE @c_EquipmentID int
DECLARE @c_TestCustomFieldID int
DECLARE @c_Value varchar(Max)
DECLARE @c_OldValue varchar(Max)
DECLARE @IsAlert bit
DECLARE @TeamMember varchar(max)
DECLARE @TeamMemberDisplay VARCHAR(MAX)
DECLARE @UserRoles VARCHAR(500)
DECLARE @UserRolesDisplay VARCHAR(MAX)
DECLARE @label VARCHAR(MAX)
SET NOCOUNT ON;    
BEGIN TRY
BEGIN TRANSACTION 
 DECLARE db_cursor1 CURSOR FOR      
	SELECT TestID, EquipmentID,TestCustomFieldID,CustomValue,CustomOldValue FROM @tblSafetyTestUpdate        
 OPEN db_cursor1      
 FETCH NEXT FROM db_cursor1 INTO @c_TestID,@c_EquipmentID,@c_TestCustomFieldID,@c_Value,@c_OldValue
    WHILE @@FETCH_STATUS = 0    
	 BEGIN  
	  if Not EXISTS (select 1 from [tblTestCustomFieldsValue] where [TestID]=@c_TestID and [EquipmentID]= @c_EquipmentID and [tblTestCustomFieldsID]= @c_TestCustomFieldID)
		  Begin
		  
			SELECT @IsAlert=[IsAlert],@TeamMember=[TeamMember],@TeamMemberDisplay=TeamMemberDisplay,@UserRoles=UserRoles,@UserRolesDisplay=UserRolesDisplay ,@label=Label
			FROM tblTestCustomFields tb
			WHERE ID=@c_TestCustomFieldID

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
           (@c_TestID
           ,@c_EquipmentID
           ,@c_TestCustomFieldID
           ,@c_Value
           ,@UpdatedBy
           ,GETDATE()
		   ,@IsAlert
		   ,@TeamMember,@TeamMemberDisplay,@UserRoles,@UserRolesDisplay)	

			 exec log2_insert @UpdatedBy,'SafetyTest',@c_TestID,@label,'',@c_Value

		  End
	  Else
		  BEGIN		 
			 UPDATE [tblTestCustomFieldsValue]
			   SET 
				  [Value] = @c_Value
				  ,[UpdatedBy] = @UpdatedBy
				  ,[UpdatedDate] = GETDATE()
				  
			 WHERE [TestID]=@c_TestID and [EquipmentID]= @c_EquipmentID and [tblTestCustomFieldsID]= @c_TestCustomFieldID
			
			SELECT @label=Label
			FROM tblTestCustomFields tb
			WHERE ID=@c_TestCustomFieldID

			 exec log2_insert @UpdatedBy,'SafetyTest',@c_TestID,@label,@c_OldValue,@c_Value
		  End
		  
			

		  FETCH NEXT FROM db_cursor1 INTO @c_TestID,@c_EquipmentID,@c_TestCustomFieldID,@c_Value,@c_OldValue
		END
CLOSE db_cursor1    
DEALLOCATE db_cursor1 
COMMIT TRANSACTION   
END TRY
BEGIN CATCH	

	CLOSE db_cursor1  
	DEALLOCATE db_cursor1 
	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	ROLLBACK	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH

END