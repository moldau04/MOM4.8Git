 
-- =============================================
-- Author:		<PoojA>
-- Create date: <14-March-2020>
-- Description:	< Create User Token>
-- =============================================
Create PROCEDURE sp_Core_CreateUserToken
	-- Add the parameters for the stored procedure here
    @Token nvarchar(500),
	@Domain_Name varchar (500),
	@User_Id int,
	@company varchar (500)
AS
BEGIN  

  INSERT INTO [dbo].[Core_UserToken]
           ([User_Id]
           ,[Token]
           ,[company]
           ,[Expiry_Date]
           ,[CreatedOn]
           ,[Domain_Name])
     VALUES
           (@User_Id
           ,@Token
           ,@company
           ,DATEADD(HOUR, 1, GETDATE())  
           ,GETDATE()   
           ,@Domain_Name )

END