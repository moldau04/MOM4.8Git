Create Procedure spCreateMissingDepositDetail
	@depId int
As
BEGIN TRY
	
	DECLARE @c_TransID INT 	

	IF (SELECT COUNT(*) FROM DepositDetails WHERE DepID=@depId) =0
	BEGIN
		DECLARE db_Tran CURSOR FOR 
			SELECT ID FROM Trans WHERE Ref=@depId and Type =6		
		OPEN db_Tran  
		FETCH NEXT FROM db_Tran INTO @c_TransID

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			INSERT INTO DepositDetails(DepID,TransID) VALUES (@depId,@c_TransID)
			FETCH NEXT FROM db_Tran INTO @c_TransID
		END 
		CLOSE db_Tran  
		DEALLOCATE db_Tran 
	END
	
END TRY
BEGIN CATCH
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH
