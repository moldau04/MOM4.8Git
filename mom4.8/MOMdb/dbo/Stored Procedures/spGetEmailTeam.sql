Create PROCEDURE  [dbo].[spGetEmailTeam]
@Email	varchar(50)
AS          
BEGIN          
	
	IF ( @Email='' )          	
		BEGIN
			SELECT DISTINCT TOP 1000 [Email]				      
			FROM team
		END
    ELSE
		BEGIN
			SELECT DISTINCT TOP 1000 [Email]				      
			FROM team WHERE [Email] LIKE '%' + @Email +'%' 
		END
END