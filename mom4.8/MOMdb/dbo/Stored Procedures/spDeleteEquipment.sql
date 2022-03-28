CREATE proc [dbo].[spDeleteEquipment]
	@equipid int
AS 

BEGIN TRY
	BEGIN TRANSACTION  
		

	IF NOT EXISTS  (select 1 from multiple_equipments where elev_id = @equipid union select 1 from tblJoinElevJob where elev=@equipid ) 
	BEGIN  

		DECLARE @loc int
		SELECT @loc = Loc 
		FROM Elev where ID=@equipid

		DELETE from Elev where ID=@equipid
		DELETE FROM EquipTItem WHERE   Elev=@equipid


		UPDATE Loc SET [Elevs] = ([Elevs] - 1) WHERE Loc = @loc
		UPDATE [Owner] SET [Elevs] = ([Elevs] - 1) WHERE ID = (select [Owner] from Loc where Loc= @Loc)	

	
		
		DELETE LoadTestItemHistory where [LID] IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)

		DELETE [dbo].LoadTestItemHistoryPrice WHERE  [LID]IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)
				
		DELETE LoadTestItemSchedule WHERE LID IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)
		
		DELETE LoadTestItemService WHERE LID IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)
		
		DELETE [dbo].TestHistory where idTest IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)
        
		DELETE FROM Log2 WHERE Ref IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)
		AND Screen='SafetyTest' 

		DELETE FROM tblTestCustomFieldsValue
		WHERE TestID  IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)

		DELETE from.ProposalFormDetail WHERE TestID IN (SELECT LID FROM LoadTestItem WHERE Elev=@equipid)

		DELETE [dbo].[LoadTestItem] where Elev =@equipid


		DECLARE @TestType int
		DECLARE cur_Item CURSOR FOR 	
			SELECT ID FROM LoadTestItem WHERE Elev=@equipid
		OPEN cur_Item  
		FETCH NEXT FROM cur_Item INTO @TestType
		WHILE @@FETCH_STATUS = 0  
			BEGIN
					UPDATE LoadTest SET Count=Count-1 WHERE ID=@TestType AND Count>0		
			FETCH NEXT FROM cur_Item INTO @TestType
			END	
		CLOSE cur_Item  
		DEALLOCATE cur_Item  


	

	END  
	ELSE 
		BEGIN  

			RAISERROR ('Selected equipment is in use, equipment cannot be deleted!', 16, 1) 
			RETURN 
		END
	COMMIT
END TRY
BEGIN CATCH		
	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	ROLLBACK	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH





