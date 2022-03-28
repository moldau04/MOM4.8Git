



CREATE PROCEDURE [dbo].[spGetItype]
@ID INT= NULL

 

as
	begin
			IF @ID IS NULL
			SELECT *
			FROM IType

			IF @ID IS NOT NULL 
			SELECT *
			FROM IType WHERE ID=@ID
		
	
	end
