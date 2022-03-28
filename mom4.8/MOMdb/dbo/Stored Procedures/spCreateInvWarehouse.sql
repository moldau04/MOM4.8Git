CREATE PROCEDURE [dbo].[spCreateInvWarehouse]
			
            @InvID int ,
			@WarehouseID varchar(5)= NULL
			

as
	begin
			DECLARE @success int
			set @success=0
		
		 if not exists(select 1 from InvWarehouse where InvID=@InvID and WareHouseID=@WareHouseID)
			 begin
			insert into InvWarehouse(InvID,WareHouseID)
			values(@InvID,@WarehouseID)
				set @success= @@IDENTITY
				end
				--else
				-- begin 
				-- set @success =': Item '+@WarehouseID+' is already exists. :'
				-- end
			

	

		select @success

	end
GO
