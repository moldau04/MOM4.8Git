CREATE PROCEDURE spAddTestSetupForms
 @Name varchar(50)  
,@FileName varchar(100)
,@FilePath varchar(500)    
,@MIME varchar(50)
,@AddedBy  varchar(50)    
,@Type int  
,@IsActive bit
,@ID int output
AS
BEGIN
IF @IsActive=1
	BEGIN
		UPDATE [TestSetupForms]
		SET [IsActive]=0
		WHERE [Type]=@Type

	END

INSERT INTO [TestSetupForms]
           ([Name]
           ,[FileName]
           ,[FilePath]           
           ,[MIME]
           ,[AddedBy]
           ,[AddedOn]
		   ,[Type]
		   ,[IsActive])
     VALUES
           (@Name
           ,@FileName
           ,@FilePath           
           ,@MIME
           ,@AddedBy
           ,GETDATE()
		   ,@Type
		   ,@IsActive)
Set @ID= @@IDENTITY
END

GO
