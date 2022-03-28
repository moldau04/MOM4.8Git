CREATE PROCEDURE [dbo].[spDeleteInvparts]
			@id int
        
as
	begin
			
		
			delete from InvParts where ID=@id
		

	end
GO
