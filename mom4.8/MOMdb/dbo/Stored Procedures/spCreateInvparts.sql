CREATE PROCEDURE [dbo].[spCreateInvparts]
			
            @ItemID int ,
			@MPN varchar(75)= NULL,
			@Part varchar(50),
			@Supplier varchar(25)= NULL,
			@VendorID int,
			@Price Float,
			@Mfg  varchar(75)=null,
			@MfgPrice Float,
			@Id int

as
	begin

			DECLARE @success int
			IF @Id = 0
			BEGIN
				set @success=0
				insert into InvParts(ItemID,MPN,Part,Supplier,VendorID,Price,Mfg,MfgPrice)
				values(@ItemID,@MPN,@Part,@Supplier,@VendorID,@Price,@Mfg,@MfgPrice)
				set @success= @@IDENTITY
				select @success
			END
			ELSE 
			BEGIN
				UPDATE InvParts SET ItemID =@ItemID,MPN= @MPN,Part =@Part,Supplier= @Supplier,VendorID= @VendorID,Price= @Price,Mfg = @Mfg,MfgPrice = @MfgPrice
				WHERE ID = @Id
				set @success= @Id
				select @success
			END
	end
GO


