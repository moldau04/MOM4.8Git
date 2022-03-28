CREATE PROCEDURE [dbo].[spUpdateTestTypeCount]			
         
			@id int,
			@operation nvarchar(100)=null
		   
		
as
	begin
		if (@operation is null or @operation='Increment')
		UPDATE LoadTest SET Count=Count+1 WHERE ID=@id
		else if ( @operation='Decrement')
			UPDATE LoadTest SET Count=Count-1 WHERE ID=@id AND Count>0

			
	end
GO