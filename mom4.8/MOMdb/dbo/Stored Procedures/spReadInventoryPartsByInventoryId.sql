CREATE PROCEDURE [dbo].[spReadInventoryPartsByInventoryId]
			@Id [int]

as
	begin
		
			select * from InvParts
		    where ItemID=@Id


			


	end
