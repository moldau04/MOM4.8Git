
CREATE PROCEDURE [dbo].[spReadInventoryManufacturerInformationByInventoryId]
			@Id [int]

as
	begin
		
			select inv.ID,inv.InventoryManufacturerInformation_InvID,inv.MPN,inv.ApprovedManufacturer,vn.ID as ApprovedVendorId,vn.Acct from InventoryManufacturerInformation inv
		inner join Vendor vn on inv.ApprovedVendor=vn.ID
		where inv.InventoryManufacturerInformation_InvID=@Id


			


	end