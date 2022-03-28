CREATE PROCEDURE [dbo].[spCreateTestByYear]				
	@typeID int
    ,@Loc int
    ,@Elev int
    ,@Last datetime =null
    ,@Next datetime =null
    ,@Status int
    ,@Ticket int =null
    ,@Remarks ntext=null
    ,@LastDue datetime=null
	,@JobId int=NULL
    ,@Custom1 varchar(50)
	,@Custom2 varchar(50)
	,@Custom3 varchar(50)
	,@Custom4 varchar(50)		  
    ,@Amount numeric(32,2)
	,@OverrideAmount numeric(32,2)
	,@ThirdPartyName varchar(50)
	,@ThirdPartyPhone varchar(50)  
	,@TestDueBy int
	,@Charge INT
	,@ThirdParty INT
    ,@PriceYear INT 
	,@CreatedBy VARCHAR(100)
	,@TestCustomItemValue   tblTypeTestCustomItemValue readonly
	,@id Int output
		
AS 
	BEGIN 

	BEGIN TRANSACTION  
	BEGIN TRY
		set @Id=(SELECT ISNULL(MAX(LID),0)+1 AS ID FROM LoadTestItem)
		INSERT INTO [dbo].[LoadTestItem]
           (
			[LID]
           ,[ID]
           ,[Loc]
           ,[Elev]
           ,[Last]
           ,[Next]
           ,[Status]
           ,[Ticket]
           ,[Remarks]
           ,[LastDue]
           ,[idRolCustomContact]
           ,[Custom1]
           ,[Custom2]
           ,[Custom3]
           ,[Custom4]
           ,[Custom5]
           ,[Custom6]
           ,[Custom7]
           ,[Custom8]
           ,[Custom9]
           ,[Custom10]
		   ,[JobId]
		   ,[Amount]
		   ,[OverrideAmount]
		   ,[ThirdPartyName]
		   ,[ThirdPartyPhone]
		   ,[TestDueBy]
		   ,[Chargeable]
		   ,[ThirdParty]		  
		   )
     VALUES
           (
			@Id
           ,@typeID
           ,@Loc
           ,@Elev
           ,@Last
           ,@Next
           ,@Status
           ,@Ticket
           ,@Remarks
           ,@LastDue
           ,0
           ,@Custom1
           ,@Custom2
           ,@Custom3
           ,@Custom4
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
		   ,@JobId
		   ,@Amount
		   ,@OverrideAmount
		   ,@ThirdPartyName
		   ,@ThirdPartyPhone
		   ,@TestDueBy
		   ,@Charge
		   ,@ThirdParty		   
		   )
				
		
		--Add LoadTestItemHistory		
		INSERT INTO LoadTestItemHistory (LID,TestStatus,Next,Last,LastDue,TestYear,isTestDefault,TicketID,JobId,IsActive)
		VALUES (@Id,0,@Next,@Last,@LastDue,YEAR(@Next),1,@Ticket,@JobId,1)
		--Add PriceHistory
		IF (SELECT COUNT(1) FROM LoadTestItemHistoryPrice WHERE LID=@id AND PriceYear=@PriceYear)=0
		BEGIN
			INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,CreatedBy,CreatedDate,[DefaultAmount],[OverrideAmount],DueDate,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)
			VALUES (@Id,@PriceYear,@CreatedBy,GETDATE(),@Amount,@OverrideAmount,@Next,@Charge,@ThirdParty,@ThirdPartyName,@ThirdPartyPhone)	
        END

		 INSERT INTO [dbo].[TestHistory]
           ( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
		VALUES
           (@ID ,GETDATE(),@CreatedBy ,'Open' ,@Last,1,null ,NULL, NULL,@Next,@LastDue)	
	

		-- Update for test cover
		DECLARE @TestTypeCover INT
		
        SET @TestTypeCover= ISNULL((SELECT TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@typeID),0)
		IF @TestTypeCover!=0
		BEGIN
			UPDATE LoadTestItem
			SET Chargeable=0 , Amount=0, OverrideAmount=0
			WHERE Elev=@Elev AND ID= @TestTypeCover AND YEAR(Next)=YEAR(@Next)
			

			UPDATE LoadTestItemHistoryPrice
			SET Chargeable=0 , DefaultAmount=0, OverrideAmount=0
			WHERE PriceYear=YEAR(@Next) AND LID= (SELECT LID FROM LoadTestItem WHERE Elev=@Elev AND ID= @TestTypeCover AND YEAR(Next)=YEAR(@Next) )
        END 

		--Update TestType count
		UPDATE LoadTest SET Count=Count+1 WHERE ID=@id

		-- Add Test custom field value
	DECLARE @c_ID int
	DECLARE @c_tblTestCustomFieldsID int
	DECLARE @c_Value varchar(50)
	DECLARE @c_UpdatedBy varchar(50)
	DECLARE @c_IsAlert bit
	DECLARE @c_TeamMember varchar(max)
	DECLARE @c_TeamMemberDisplay VARCHAR(MAX)
	DECLARE @c_UserRoles VARCHAR(500)
	DECLARE @c_UserRolesDisplay VARCHAR(MAX)
	DECLARE @c_label VARCHAR(MAX)
	DECLARE @c_oldValue VARCHAR(MAX)
	DECLARE db_cursor1 CURSOR FOR      
	SELECT [ID],[tblTestCustomFieldsID],[Value],[UpdatedBy],[IsAlert],[TeamMember],TeamMemberDisplay,UserRoles,UserRolesDisplay FROM @TestCustomItemValue        
	OPEN db_cursor1      
	FETCH NEXT FROM db_cursor1 INTO @c_ID, @c_tblTestCustomFieldsID,@c_Value, @c_UpdatedBy,@c_IsAlert,@c_TeamMember,@c_TeamMemberDisplay,@c_UserRoles,@c_UserRolesDisplay
    WHILE @@FETCH_STATUS = 0    
	 BEGIN 
	  
		  BEGIN
			  IF (@c_Value<>'')
				  BEGIN
					INSERT INTO [tblTestCustomFieldsValue]
				   ([TestID]
				   ,[EquipmentID]
				   ,[tblTestCustomFieldsID]
				   ,[Value]
				   ,[UpdatedBy]
				   ,[UpdatedDate]
				   ,IsAlert
				   ,TeamMember,TeamMemberDisplay,UserRoles,UserRolesDisplay,TestYear)
				 VALUES
				   (@ID
				   ,@Elev
				   ,@c_tblTestCustomFieldsID
				   ,@c_Value
				   ,@c_UpdatedBy
				   ,GETDATE()
				   ,@c_IsAlert
				   ,@c_TeamMember,@c_TeamMemberDisplay,@c_UserRoles,@c_UserRolesDisplay,@PriceYear)		   
			

					SELECT @c_label=Label
					FROM tblTestCustomFields tb
					WHERE ID=@c_tblTestCustomFieldsID

					 exec log2_insert @c_UpdatedBy,'SafetyTest',@ID,@c_label,'',@c_Value
				  END
		  END

		 FETCH NEXT FROM db_cursor1 INTO @c_ID, @c_tblTestCustomFieldsID,@c_Value, @c_UpdatedBy,@c_IsAlert,@c_TeamMember,@c_TeamMemberDisplay,@c_UserRoles,@c_UserRolesDisplay
	 END 

	CLOSE db_cursor1    
	DEALLOCATE db_cursor1 

		--Add Log
	
		if(@loc is not null And @loc != 0)
		Begin 	
		Declare @CurrentLocation varchar(100)
		Select @CurrentLocation = tag from loc where loc = @loc
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Location Name','',@CurrentLocation
		END

		if(@typeID is not null And @typeID != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Test Type','',@typeID
		END




		if(@Elev is not null And @Elev != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Equipment','',@Elev
		END

		if(@Last is not null And @Last != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Last Due Date','',@Last
		END

		IF(@Next is not null And @Next != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Next Due Date','',@Next
		END

		IF(@Status is not null And @Status != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Test Status','',@Status
		END
  
	IF(@Ticket is not null And @Ticket != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Ticket','',@Ticket
		END
   
		IF(@Remarks is not null  )
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Remarks','',@Remarks
		END

		  IF(@LastDue is not null And @LastDue != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'LastDue','',@LastDue
		END

		 IF(@JobId is not null And @JobId != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Project','',@JobId
		END

				 IF(@Custom1 is not null And @Custom1 != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Custom1','',@Custom1
		END

		IF(@Custom2 is not null And @Custom2 != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Custom2','',@Custom2
		END

		IF(@Custom3 is not null And @Custom3 != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Custom3','',@Custom3
		END

		IF(@Custom4 is not null And @Custom4 != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Custom4','',@Custom4
		END
  
	IF(@Amount is not null)
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Default Amount','',@Amount
		END

		IF(@OverrideAmount is not null)
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Override Amount','',@OverrideAmount
		END
  
	
		IF(@ThirdPartyName is not null And @ThirdPartyName != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Third Party Name','',@ThirdPartyName
		END

		IF(@ThirdPartyPhone is not null And @ThirdPartyPhone != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Third Party Phone','',@ThirdPartyPhone
		END

		IF(@TestDueBy is not null And @TestDueBy != '')
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Test Due By','',@TestDueBy
		END

		IF(@Charge is not null )
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Chargeable','',@Charge
		END

		IF(@ThirdParty is not null )
		Begin 	
		exec log2_insert @CreatedBy,'SafetyTest',@Id,'Third Party','',@ThirdParty
		END	

		

	COMMIT
	return @Id
END TRY
BEGIN CATCH	

ROLLBACK	
	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH




		
	
	END 