
CREATE PROCEDURE [dbo].[spUpdateInventoryManufacturerInformation]
			@Id [int],
           @InventoryManufacturerInformation_InvID [int] ,
			@MPN [varchar](75)= NULL,
			@ApprovedManufacturer[varchar](75)= NULL,
			@ApprovedVendor [varchar](75) =NULL

as
	begin
		
			update InventoryManufacturerInformation set 
			InventoryManufacturerInformation.InventoryManufacturerInformation_InvID=@InventoryManufacturerInformation_InvID,
			InventoryManufacturerInformation.ApprovedManufacturer=@ApprovedManufacturer,
			InventoryManufacturerInformation.ApprovedVendor=@ApprovedVendor,
			InventoryManufacturerInformation.MPN=@MPN
			
			where InventoryManufacturerInformation.ID=@Id


			


	end