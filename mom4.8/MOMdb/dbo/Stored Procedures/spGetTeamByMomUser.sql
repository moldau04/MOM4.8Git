Create PROCEDURE  [dbo].[spGetTeamByMomUser]
@user	varchar(50)
AS          
BEGIN          
	SET NOCOUNT ON;
	IF ( @user='' )          	
		BEGIN
			SELECT TOP 1000
				   [ID]
				  ,[JobID]
				  ,[Line]
				  ,[Title]
				  ,[MomUserID]
				  ,[FirstName]
				  ,[LastName]
				  ,[Email]
				  ,[Mobile]           
			FROM team 
		END
    ELSE
		BEGIN
			SELECT TOP 1000
				[ID]
				  ,[JobID]
				  ,[Line]
				  ,[Title]
				  ,[MomUserID]
				  ,[FirstName]
				  ,[LastName]
				  ,[Email]
				  ,[Mobile]      
			FROM team WHERE MomUserID LIKE '%' + @user +'%' 
		END
END