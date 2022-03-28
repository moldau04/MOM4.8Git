
CREATE PROCEDURE [dbo].[spDeleteInventoryManufacturerInformation]
			@id int
        
as
	begin
			--SELECT * FROM InventoryManufacturerInformation WITH (NOLOCK) WHERE InventoryManufacturerInformation.ID=8
		
			delete from InventoryManufacturerInformation where InventoryManufacturerInformation.ID=@id
		

	end