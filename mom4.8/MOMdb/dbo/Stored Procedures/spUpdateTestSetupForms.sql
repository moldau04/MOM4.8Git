Create Procedure spUpdateTestSetupForms
 @Name varchar(50)  
,@FileName varchar(100)
,@FilePath varchar(500)    
,@MIME varchar(50)
,@UpdatedBy  varchar(50)    
,@ID int
,@Type INT
,@IsActive bit
AS
BEGIN
IF @IsActive=1
	BEGIN
		UPDATE [TestSetupForms]
		SET [IsActive]=0
		WHERE [Type]=@Type

	END

UPDATE [TestSetupForms]
   SET [Name] =@Name  
      ,[FileName]=@FileName  
      ,[FilePath]=@FilePath      
      ,[MIME] =@MIME
      ,[UpdatedBy] = @UpdatedBy
      ,[UpdatedOn] = GETDATE()
	  ,[Type]=@Type
	  ,[IsActive]=@IsActive
 WHERE ID=@ID
END

GO
