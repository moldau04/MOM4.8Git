CREATE PROCEDURE [dbo].[spDeleteTest]			
     @id int
AS
	
BEGIN TRY
	BEGIN TRANSACTION  
		

		Delete [dbo].[LoadTestItem] where [LID]=@id
		
		DELETE LoadTestItemHistory where [LID]=@id

		DELETE [dbo].LoadTestItemHistoryPrice where [LID]=@id
				
		DELETE LoadTestItemSchedule WHERE LID=@id
		
		DELETE LoadTestItemService WHERE LID=@id
		
		DELETE [dbo].TestHistory where idTest=@id
        
		DELETE FROM Log2 WHERE Ref=@id AND Screen='SafetyTest' 

		UPDATE LoadTest SET Count=Count-1 WHERE ID=@id AND Count>0		

		DELETE from.ProposalFormDetail WHERE TestID=@id

		DELETE FROM tblTestCustomFieldsValue
		WHERE TestID =@id

	COMMIT
END TRY
BEGIN CATCH		
	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
	ROLLBACK	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH



