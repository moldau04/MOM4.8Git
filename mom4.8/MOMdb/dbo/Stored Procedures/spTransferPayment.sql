CREATE Procedure [dbo].[spTransferPayment] (
            @newLoc      INT 
           ,@lsRef   VARCHAR(500)
          
)

AS BEGIN
	DECLARE @c_Ref VARCHAR (100)
	DECLARE @owner INT
    DECLARE @oldLoc int
	DECLARE db_Ref CURSOR FOR 
		select Item from dbo.SplitString(@lsRef,';')
	OPEN db_Ref  
	FETCH NEXT FROM db_Ref INTO @c_Ref
		
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	IF @c_Ref!=''
		BEGIN
		    SET @oldLoc=(SELECT loc FROM OpenAR WHERE REF= CAST(@c_Ref AS INT) AND Type=2)
			SET @owner=(SELECT owner FROM Loc WHERE Loc=@newLoc)

			UPDATE ReceivedPayment
			SET Owner=@owner, loc=@newLoc
			WHERE ID= CAST(@c_Ref AS INT)

			UPDATE OpenAR
			SET loc=@newLoc
			WHERE REF= CAST(@c_Ref AS INT) AND Type=2

			UPDATE Trans
			SET AcctSub=@newLoc
			WHERE ID =(SELECT TransID FROM OpenAR WHERE REF= CAST(@c_Ref AS INT) AND Type=2)
		END
		

		FETCH NEXT FROM db_Ref INTO @c_Ref
	END
	CLOSE db_Ref  
	DEALLOCATE db_Ref 
	EXEC spUpdateCustomerLocBalance @oldLoc, 0	
	EXEC  spUpdateCustomerLocBalance @newLoc, 0	
END

