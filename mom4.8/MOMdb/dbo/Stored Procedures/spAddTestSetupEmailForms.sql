CREATE PROCEDURE spAddTestSetupEmailForms
 @Name varchar(50)  
,@Body varchar(MAX)
,@AddedBy  varchar(50)    
,@IsActive bit
,@ID int output
AS
BEGIN
if (select count(1) from TestSetupEmailForms where Name=@Name)=0
BEGIN
	INSERT INTO [TestSetupEmailForms]
           ([Name]
           ,[Body]           
           ,[AddedBy]
           ,[AddedOn]		  
		   ,[IsActive])
     VALUES
           (@Name
           ,@Body          
           ,@AddedBy
           ,GETDATE()		  
		   ,@IsActive)
	Set @ID= @@IDENTITY
	--Update Active
	if (@IsActive=1)
	begin
			UPDATE [TestSetupEmailForms]
		SET [IsActive]=0
		WHERE ID!=@ID
	END
END
ELSE
BEGIN
	Set @ID=-1 --Template Name exist
END


END

GO
