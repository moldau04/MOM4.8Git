CREATE PROCEDURE [dbo].[spUpdateInvparts]
			
			@Id int,
            @ItemID int ,
			@MPN varchar(75)= NULL,
			@Part varchar(50),
			@Supplier varchar(25)= NULL,
			@VendorID int,
			@Price Float,
			@Mfg  varchar(75)=null,
			@MfgPrice Float

as
	begin
		
			update InvParts  set 
			ItemID=@ItemID,MPN=@MPN,Part=@Part,Supplier=@Supplier,VendorID=@VendorID,Price=@Price,Mfg=@Mfg,MfgPrice=@MfgPrice
			
			
			where ID=@Id


			


	end
GO


