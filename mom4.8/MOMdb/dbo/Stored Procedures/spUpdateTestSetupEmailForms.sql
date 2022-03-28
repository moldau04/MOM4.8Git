CREATE Procedure spUpdateTestSetupEmailForms
 @Name varchar(50)  
,@Body varchar(100)
,@UpdatedBy  varchar(50) 
,@IsActive bit
,@ID Int
,@Result int output
AS
BEGIN
Declare @Old_Name varchar(100)
Set @Old_Name=(select Name from [TestSetupEmailForms] where ID=@ID)
if @Old_Name=@Name
BEGIN
	UPDATE [TestSetupEmailForms]
	   SET 
		  [Body]=@Body		  
		  ,[UpdatedBy] = @UpdatedBy
		  ,[UpdatedOn] = GETDATE()		 
		  ,[IsActive]=@IsActive
		WHERE ID=@ID
		if (@IsActive=1)
		begin
				UPDATE [TestSetupEmailForms]
			SET [IsActive]=0
			WHERE ID!=@ID
		END
		Set @Result=1
END
ELSE
BEGIN
	if (select count(1) from TestSetupEmailForms where Name=@Name)=0
	BEGIN
		UPDATE [TestSetupEmailForms]
	   SET 
		  [Body]=@Body		  
		  ,[UpdatedBy] = @UpdatedBy
		  ,[UpdatedOn] = GETDATE()		 
		  ,[IsActive]=@IsActive
		WHERE ID=@ID
		if (@IsActive=1)
		begin
				UPDATE [TestSetupEmailForms]
			SET [IsActive]=0
			WHERE ID!=@ID
		END
		Set @Result=1
	END
	ELSE
	BEGIN
		Set @Result=-1
	END
END


END

