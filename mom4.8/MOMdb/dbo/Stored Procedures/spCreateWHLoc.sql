CREATE PROCEDURE [dbo].[spCreateWHLoc]
	@WareHouseID varchar(5),

	@WareHouseLocation Varchar(75),

	@IsEdit bit
	
AS
BEGIN

			DECLARE @success varchar(50)
			begin try
			if not exists(select 1 from Warehouse where ID=@WareHouseID and @IsEdit=0)
				Begin
					if not exists(select 1 from WHLoc where Name=@WareHouseLocation and WareHouseID=@WareHouseID)
					BEGIN
						INSERT INTO [dbo].[WHLoc]
							   (
							   [Name]
							   ,[WareHouseID]
							   )
								 VALUES
							   (@WareHouseLocation,@WareHouseID)
						set @success='success'
				   END
				else
					begin
						
						set @success ='Location '+@WareHouseLocation+' could not be created as it already exists.'
							
					end
				END
			else
				begin
					
					set @success='Warehouse '+@WareHouseID+' is already exist.'
					
				end
			end try
			
			begin catch
			IF @@ERROR >0
				 BEGIN  
					set @success='-1'
				 END
			end catch

		select @success
END


